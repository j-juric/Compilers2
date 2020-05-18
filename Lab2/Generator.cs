using Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Type = Syntax.Type;

namespace Lab2
{
    //public class Type
    //{
    //    public static Type Instance = new Type();
    //}

    // ---

    public class FunctionGeneratorEnvironment
    {
        public string FunctionName;
        public Dictionary<string, int> VariableOffsetEnvironment = new Dictionary<string, int>();
        public Dictionary<string, Type> VariableType = new Dictionary<string, Type>();
        public int NextVariableOffset = -1;
        public int NextArgumentOffset = 2;
        public int NextLabelNumber = 0;

        public FunctionGeneratorEnvironment()
        {
            FunctionName = "";
        }

        public FunctionGeneratorEnvironment(string name)
        {
            FunctionName = name;
        }

        public void MarkAsLocalFunction(string name)
        {
            VariableOffsetEnvironment[name] = 0;
        }

        public bool IsLocalFunction(string name)
        {
            if (VariableOffsetEnvironment.ContainsKey(name))
            {
                return VariableOffsetEnvironment[name] == 0;
            }

            return false;
        }

        public int AllocateVariable(string name, Type type)
        {
            var offset = NextVariableOffset;
            VariableOffsetEnvironment[name] = offset;
            VariableType[name] = type;
            NextVariableOffset--;
            return offset;
        }

        public void AllocateArgument(string name)
        {
            var offset = NextArgumentOffset;
            VariableOffsetEnvironment[name] = offset;
            NextArgumentOffset++;
        }

        public int GetIdentifierOffset(string name)
        {
            return VariableOffsetEnvironment[name];
        }

        public Type GetIdentifierType(string name)
        {
            return VariableType[name];
        }

        // should only be called after all arguments are allocated
        public int GetReturnValueOffset()
        {
            return NextArgumentOffset;
        }

        public int GetNextLabelNumber()
        {
            return NextLabelNumber++;
        }
    }
    public class Generator : IExpressionVisitorWithArguments<Type, FunctionGeneratorEnvironment>
    {
        public T42Program Program = new T42Program();


        enum OperatorExpression
        {
            LOGICAL,
            INTEGER,
            EQUALITY,
            INEQUALITY
        }


        Dictionary<BinOperatorStatement.Type, OperatorExpression> MapOpExpr = new Dictionary<BinOperatorStatement.Type, OperatorExpression>()
        {
            {BinOperatorStatement.Type.ADD, OperatorExpression.INTEGER },
            {BinOperatorStatement.Type.SUB, OperatorExpression.INTEGER },
            {BinOperatorStatement.Type.MUL, OperatorExpression.INTEGER },
            {BinOperatorStatement.Type.DIV, OperatorExpression.INTEGER },
            {BinOperatorStatement.Type.LE, OperatorExpression.INEQUALITY },
            {BinOperatorStatement.Type.LEQ, OperatorExpression.INEQUALITY },
            {BinOperatorStatement.Type.GR, OperatorExpression.INEQUALITY },
            {BinOperatorStatement.Type.GEQ, OperatorExpression.INEQUALITY },
            {BinOperatorStatement.Type.AND, OperatorExpression.LOGICAL },
            {BinOperatorStatement.Type.OR, OperatorExpression.LOGICAL },
            {BinOperatorStatement.Type.EQ, OperatorExpression.EQUALITY },
            {BinOperatorStatement.Type.NEQ, OperatorExpression.EQUALITY },
        };


        public T42Program Generate(Statement stmt)
        {
            var globalEnvironment = new FunctionGeneratorEnvironment();
            Program.Emit(T42Instruction.DECL(1));
            Program.Emit(T42Instruction.BSR("main"));
            Program.Emit(T42Instruction.END);
            stmt.Accept(this, globalEnvironment);          
            Program.Link();
            return Program;
        }

        public Type Visit(DeclarationSequence declarationSequence, FunctionGeneratorEnvironment arg)
        {

            var declaration = declarationSequence;

            while (declaration != null)
            {
                var typeName = declaration.tail.GetType().Name;
                if (typeName == "TypeDeclaration")
                    ((TypeDeclaration)declaration.tail).Accept(this,arg);
                else
                    ((TypeDeclaration)declaration.tail).Accept(this,arg);

                declaration = (DeclarationSequence)declaration.head;
            }

            return null;
        }

        public Type Visit(SequenceStatement sequenceStatement, FunctionGeneratorEnvironment arg)
        {
            if (sequenceStatement.head != null)
            {
                ((SequenceStatement)sequenceStatement.head).Accept(this,arg);
            }
            sequenceStatement.tail.Accept(this,arg);

            return null;
        }

        public Type Visit(IdentifierStatement identifierStatement, FunctionGeneratorEnvironment arg)
        {

            if (identifierStatement.list == null)
            {
                var type = arg.GetIdentifierType(identifierStatement.id);
                var offset = arg.GetIdentifierOffset(identifierStatement.id);
                if(type.GetType() == typeof(IntType))
                {
                    Program.Emit(T42Instruction.RVALINT(offset));
                    return new IntType();
                }
                else
                {
                    Program.Emit(T42Instruction.RVALBOOL(offset));
                    return new BoolType();
                }
            }
            else
            {
                // CALL STATEMENT
                return new BoolType();
            }



        }


        public Type Visit(BinOperatorStatement binOperatorStatement, FunctionGeneratorEnvironment arg)
        {
            var left = binOperatorStatement.left;
            var right = binOperatorStatement.right;
            var type = binOperatorStatement.type;
            var opType = MapOpExpr[binOperatorStatement.type];

            switch (opType)
            {
                case OperatorExpression.INTEGER:
                    left.Accept(this, arg);
                    right.Accept(this, arg);
                    switch (type)
                    {
                        
                        case BinOperatorStatement.Type.ADD:
                            Program.Emit(T42Instruction.ADD);
                            break; 
                        case BinOperatorStatement.Type.SUB:
                            Program.Emit(T42Instruction.SUB);
                            break;
                        case BinOperatorStatement.Type.MUL:
                            Program.Emit(T42Instruction.MUL);
                            break;
                        case BinOperatorStatement.Type.DIV:
                            Program.Emit(T42Instruction.DIV);
                            break;
                    }
                    break;

                case OperatorExpression.LOGICAL:
                    var labelno_next = arg.GetNextLabelNumber();
                    var labelno_end = arg.GetNextLabelNumber();
                    var end = $"{arg.FunctionName}_LABEL{labelno_end}";
                    var next = $"{arg.FunctionName}_LABEL{labelno_next}";
                    switch (binOperatorStatement.type)
                    {
                        case BinOperatorStatement.Type.OR:

                            left.Accept(this, arg);
                            Program.Emit(T42Instruction.BRF(next));
                            Program.Emit(T42Instruction.PUSHBOOL(1));
                            Program.Emit(T42Instruction.BRA(end));
                            Program.Emit(T42Instruction.LABEL(next));
                            right.Accept(this, arg);
                            Program.Emit(T42Instruction.LABEL(end));
                            
                            break;
                        case BinOperatorStatement.Type.AND:
                            
                            left.Accept(this, arg);
                            Program.Emit(T42Instruction.BRF(next));
                            right.Accept(this, arg);
                            Program.Emit(T42Instruction.BRA(end));
                            Program.Emit(T42Instruction.LABEL(next));
                            Program.Emit(T42Instruction.PUSHBOOL(0));
                            Program.Emit(T42Instruction.LABEL(end));
                            
                            break;
                    }
                    break;

                case OperatorExpression.INEQUALITY:
                    left.Accept(this, arg);
                    right.Accept(this, arg);
                    switch (type)
                    {
                        case BinOperatorStatement.Type.LE:
                        case BinOperatorStatement.Type.GEQ:
                            Program.Emit(T42Instruction.LTINT);
                            if (type == BinOperatorStatement.Type.GEQ)
                                Program.Emit(T42Instruction.NOT);
                            break;

                        case BinOperatorStatement.Type.GR:
                        case BinOperatorStatement.Type.LEQ:
                            Program.Emit(T42Instruction.GTINT);
                            if (type == BinOperatorStatement.Type.LEQ)
                                Program.Emit(T42Instruction.NOT);
                            break;
                    }
                    
                    break;

                case OperatorExpression.EQUALITY:
                    var t = left.Accept(this, arg);
                    right.Accept(this, arg);

                    if (t.GetType() == typeof(IntType))
                    {
                        Program.Emit(T42Instruction.EQINT);
                    }
                    else
                    {
                        Program.Emit(T42Instruction.EQBOOL);
                    }

                    if(type== BinOperatorStatement.Type.NEQ)
                        Program.Emit(T42Instruction.NOT);

                    break;
                default:
                    throw new Exception();
            }


            return new BoolType();
        }

        public Type Visit(BlockStatement blockStatement, FunctionGeneratorEnvironment arg)
        {
            blockStatement.stmt.Accept(this, arg);
            return null;
        }

        public Type Visit(Argument argument, FunctionGeneratorEnvironment arg)
        {
            arg.AllocateVariable(argument.id,(Type)argument.type);
            Program.Emit(T42Instruction.DECL(1));

            return (Type)argument.type;
        }

        public Type Visit(TypeDeclaration typeDeclaration, FunctionGeneratorEnvironment arg)
        {
            return null;
        }

        public Type Visit(VoidDeclaration voidDeclaration, FunctionGeneratorEnvironment arg)
        {
            return null;
        }


        public Type Visit(IfStatement ifStatement, FunctionGeneratorEnvironment arg)
        {           
            var labelno_end = arg.GetNextLabelNumber();
            var end = $"{arg.FunctionName}_LABEL{labelno_end}";

            ifStatement.expr.Accept(this, arg);
            Program.Emit(T42Instruction.BRF(end));

            ifStatement.stmt.Accept(this, arg);

            Program.Emit(T42Instruction.LABEL(end));

            return null;
        }

        public Type Visit(IfElseStatement ifElseStatement, FunctionGeneratorEnvironment arg)
        {
            var labelno_else = arg.GetNextLabelNumber();
            var labelno_end = arg.GetNextLabelNumber();
            var _else = $"{arg.FunctionName}_LABEL{labelno_else}";
            var end = $"{arg.FunctionName}_LABEL{labelno_end}";

            ifElseStatement.expr.Accept(this, arg);
            Program.Emit(T42Instruction.BRF(_else));

            ifElseStatement.stmt1.Accept(this, arg);
            Program.Emit(T42Instruction.BRA(end));

            Program.Emit(T42Instruction.LABEL(_else));
            ifElseStatement.stmt2.Accept(this, arg);

            Program.Emit(T42Instruction.LABEL(end));

            return null;
        }

        public Type Visit(WhileStatement whileStatement, FunctionGeneratorEnvironment arg)
        {
            var labelno_while = arg.GetNextLabelNumber();
            var labelno_end = arg.GetNextLabelNumber();
            var _while = $"{arg.FunctionName}_LABEL{labelno_while}";
            var end = $"{arg.FunctionName}_LABEL{labelno_end}";

            Program.Emit(T42Instruction.LABEL(_while));
            whileStatement.expr.Accept(this, arg);
            Program.Emit(T42Instruction.BRF(end));

            whileStatement.stmt.Accept(this, arg);

            Program.Emit(T42Instruction.BRA(_while));
            Program.Emit(T42Instruction.LABEL(end));

            return null;
        }

        public Type Visit(RegularStatement regularStatement, FunctionGeneratorEnvironment arg)
        {
            regularStatement.expr.Accept(this,arg);
            Program.Emit(T42Instruction.POP(1));

            return null;
        }

        public Type Visit(Return rreturn, FunctionGeneratorEnvironment arg)
        {
            throw null;
        }

        public Type Visit(VoidReturn voidReturn, FunctionGeneratorEnvironment arg)
        {
            return null;
        }

        public Type Visit(AssignStatement assignStatement, FunctionGeneratorEnvironment arg)
        {
            var type = arg.GetIdentifierType(assignStatement.id);
            var offset = arg.GetIdentifierOffset(assignStatement.id);

            Program.Emit(T42Instruction.LVAL(offset));
            assignStatement.s.Accept(this, arg);

            if(type.GetType() == typeof(IntType))
            {
                Program.Emit(T42Instruction.ASSINT);
                Program.Emit(T42Instruction.RVALINT(offset));
            }
            else
            {
                Program.Emit(T42Instruction.ASSBOOL);
                Program.Emit(T42Instruction.RVALBOOL(offset));
            }

            return type;
        }

        public Type Visit(NotStatement notStatement, FunctionGeneratorEnvironment arg)
        {
            notStatement.s1.Accept(this, arg);
            Program.Emit(T42Instruction.NOT);

            return new BoolType();
        }

        public Type Visit(NegativeStatement negativeStatement, FunctionGeneratorEnvironment arg)
        {
            negativeStatement.s1.Accept(this, arg);
            Program.Emit(T42Instruction.NEG);

            return new IntType();
        }

        public Type Visit(NumStatement numStatement, FunctionGeneratorEnvironment arg)
        {
            Program.Emit(T42Instruction.PUSHINT(numStatement.num));

            return new IntType();
        }

        public Type Visit(BoolStatement boolStatement, FunctionGeneratorEnvironment arg)
        {
            var val = (boolStatement.value == true) ? 1 : 0;

            Program.Emit(T42Instruction.PUSHBOOL(val));

            return new BoolType();
        }








        public Type Visit(Syntax.Type type, FunctionGeneratorEnvironment arg)
        {
            throw new NotImplementedException();
        }

        public Type Visit(IntType intType, FunctionGeneratorEnvironment arg)
        {
            throw new NotImplementedException();
        }

        public Type Visit(BoolType boolType, FunctionGeneratorEnvironment arg)
        {
            throw new NotImplementedException();
        }

        public Type Visit(ListStatement listStatement, FunctionGeneratorEnvironment arg)
        {
            throw new NotImplementedException();
        }

        public Type Visit(FormalList formalList, FunctionGeneratorEnvironment arg)
        {
            throw new NotImplementedException();
        }

        public Type Visit(Declaration declaration, FunctionGeneratorEnvironment arg)
        {
            throw new NotImplementedException();
        }

















        //public Type Visit(DeclarationSequence declarationSequence)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(SequenceStatement sequenceStatement)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(IdentifierStatement identifierStatement)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(BlockStatement blockStatement)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(BinOperatorStatement binOperatorStatement)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(Argument argument)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(TypeDeclaration typeDeclaration)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(TypeDeclaration TypeDeclaration)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(IfStatement ifStatement)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(IfElseStatement ifElseStatement)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(WhileStatement whileStatement)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(RegularStatement regularStatement)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(Return rreturn)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(TypeReturn TypeReturn)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(AssignStatement assignStatement)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(NotStatement notStatement)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(NegativeStatement negativeStatement)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(NumStatement numStatement)
        //{
        //    throw new NotImplementedException();
        //}

        //public Type Visit(BoolStatement boolStatement)
        //{
        //    throw new NotImplementedException();
        //}








        //public Type Visit(IntType intType)
        //{
        //    throw new NotImplementedException();
        //}
        //public Type Visit(BoolType boolType)
        //{
        //    throw new NotImplementedException();
        //}
        //public Type Visit(Syntax.Type type)
        //{
        //    throw new NotImplementedException();
        //}
        //public Type Visit(ListStatement listStatement)
        //{
        //    throw new NotImplementedException();
        //}
        //public Type Visit(FormalList formalList)
        //{
        //    throw new NotImplementedException();
        //}
        //public Type Visit(Declaration declaration)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
