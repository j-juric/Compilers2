using Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using Type = Syntax.Type;
namespace Lab2
{

    public class TypeEnvironment
    {
        public Dictionary<string, Type> VariableType = new Dictionary<string, Type>();
        public Dictionary<string, Declaration> FuncDeclaration = new Dictionary<string, Declaration>();
        public Type ReturnType;
    }

    public class TypeErrorMessage : ErrorMessage
    {
        public override string ErrorOutput(ErrorCode ec, Locatable loc)
        {
            return $"fail {loc.line.ToString()} {loc.column.ToString()}";
        }
    }
    public class TypeException : Exception
    {
        public TypeException(string msg) : base(msg)
        {
        }
    }

    public class TypeCheckerVisitor : IExpressionVisitor<Type>
    {
        TypeEnvironment Environment = new TypeEnvironment();


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




        bool IsVoidType(string type_text)
        {
            return (type_text != "IntType" && type_text != "BoolType");
        }

        public Type Visit(DeclarationSequence declarationSequence)
        {
            var declaration = declarationSequence;
            while (declaration != null)
            {
                var typeName = declaration.tail.GetType().Name;

                if (typeName == "VoidDeclaration")
                    Environment.FuncDeclaration.Add(((VoidDeclaration)declaration.tail).id, ((VoidDeclaration)declaration.tail));
                else
                    Environment.FuncDeclaration.Add(((TypeDeclaration)declaration.tail).id, ((TypeDeclaration)declaration.tail));
                declaration = (DeclarationSequence)declaration.head;

            }

            declaration = declarationSequence;

            while (declaration != null)
            {
                var typeName = declaration.tail.GetType().Name;
                if (typeName == "VoidDeclaration")
                    ((VoidDeclaration)declaration.tail).Accept(this);
                else
                    ((TypeDeclaration)declaration.tail).Accept(this);

                Environment.VariableType.Clear();
                declaration = (DeclarationSequence)declaration.head;
            }

            return null;
        }

        public Type Visit(SequenceStatement sequenceStatement)
        {
            if (sequenceStatement.head != null)
            {
                ((SequenceStatement)sequenceStatement.head).Accept(this);
            }
            sequenceStatement.tail.Accept(this);
            return null;
        }

        public Type Visit(IdentifierStatement identifierStatement)
        {
            if (identifierStatement.list == null)
            {
                if (!Environment.VariableType.ContainsKey(identifierStatement.id))
                {
                    var err = new TypeErrorMessage();
                    throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.ID, identifierStatement));

                }
                return Environment.VariableType[identifierStatement.id];
            }
            else
            {
                if(identifierStatement.id == "print")
                {
                    foreach (var e in ((ListStatement)identifierStatement.list).exprs)
                    {
                        var type = e.Accept(this);
                        var type_text = type.GetType().Name;
                        if(type_text != "BoolType" && type_text != "IntType")
                        {
                            var err = new TypeErrorMessage();
                            throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.CALL_3, identifierStatement));

                        }
                    }
                    return null;
                }
                else
                {
                    if (!Environment.FuncDeclaration.ContainsKey(identifierStatement.id))
                    {
                        var err = new TypeErrorMessage();
                        throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.CALL_1, identifierStatement));

                    }

                    var function = Environment.FuncDeclaration[identifierStatement.id];

                    if (function.GetType().Name == "VoidDeclaration")
                    {
                        if(((FormalList)((VoidDeclaration)function).flist).list.Count != ((ListStatement)identifierStatement.list).exprs.Count)
                        {
                            var err = new TypeErrorMessage();
                            throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.CALL_2, identifierStatement));

                        }

                        var numOfArgs = ((ListStatement)identifierStatement.list).exprs.Count;

                        for (int i=0; i<numOfArgs; i++)
                        {
                            var arg = ((FormalList)((VoidDeclaration)function).flist).list[i];
                            var e = ((ListStatement)identifierStatement.list).exprs[i];
                            if(e.Accept(this).GetType() != arg.type.GetType())
                            {
                                var err = new TypeErrorMessage();
                                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.CALL_3, identifierStatement));

                            }
                        }

                        return null;
                    }
                    else
                    {
                        if (((FormalList)((TypeDeclaration)function).flist).list.Count != ((ListStatement)identifierStatement.list).exprs.Count)
                        {
                            var err = new TypeErrorMessage();
                            throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.CALL_2, identifierStatement));

                        }

                        var numOfArgs = ((ListStatement)identifierStatement.list).exprs.Count;

                        for (int i = 0; i < numOfArgs; i++)
                        {
                            var arg = ((FormalList)((TypeDeclaration)function).flist).list[i];
                            var e = ((ListStatement)identifierStatement.list).exprs[i];
                            if (e.Accept(this).GetType() != arg.type.GetType())
                            {
                                var err = new TypeErrorMessage();
                                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.CALL_3, identifierStatement));

                            }
                        }

                        return (Type)((TypeDeclaration)function).type;
                    }

                    

                }
            }
            
            
        }

        public Type Visit(NumStatement numStatement)
        {
            return new IntType();
        }

        public Type Visit(BlockStatement blockStatement)
        {
            blockStatement.stmt.Accept(this);
            return null;
        }

        public Type Visit(BinOperatorStatement binaryOperatorExpression)
        {
            var left = binaryOperatorExpression.left.Accept(this);
            var right = binaryOperatorExpression.right.Accept(this);

            var opType = MapOpExpr[binaryOperatorExpression.type];


            if (opType == OperatorExpression.INTEGER)
            {
                if (left.GetType().Name == "IntType" && right.GetType().Name == "IntType")
                {
                    return new IntType();
                }
                else
                {
                    var err = new TypeErrorMessage();
                    throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.BOP_1, binaryOperatorExpression));
                }
            }

            else if (opType == OperatorExpression.LOGICAL)
            {
                if (left.GetType().Name == "BoolType" && right.GetType().Name == "BoolType")
                {
                    return new BoolType();
                }
                else
                {
                    var err = new TypeErrorMessage();
                    TypeErrorMessage.ErrorCode code = (binaryOperatorExpression.type == BinOperatorStatement.Type.OR) ? TypeErrorMessage.ErrorCode.BOP_2 : TypeErrorMessage.ErrorCode.BOP_3;
                    throw new TypeException(err.ErrorOutput(code, binaryOperatorExpression));
                }
            }
            else if (opType == OperatorExpression.EQUALITY)
            {
                if (left.GetType().Name == "BoolType" && right.GetType().Name == "BoolType" )
                {
                    return new BoolType();
                }
                else if (left.GetType().Name == "IntType" && right.GetType().Name == "IntType")
                {
                    return new BoolType();
                }
                else
                {
                    var err = new TypeErrorMessage();
                    throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.BOP_4, binaryOperatorExpression));
                }
            }
            else if (opType == OperatorExpression.INEQUALITY)
            {
                if (left.GetType().Name == "IntType" && right.GetType().Name == "IntType")
                {
                    return new BoolType();
                }
                else
                {
                    var err = new TypeErrorMessage();
                    throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.BOP_5, binaryOperatorExpression));
                }
            }
            else
            {
                return null;               
            }

        }

        public Type Visit(ListStatement listStatement)
        {
            return null;
        }

        public Type Visit(FormalList formalList)
        {
            return null;
        }

        public Type Visit(Declaration declaration)
        {
            return null;
        }

        public Type Visit(Argument argument)
        {
            if (Environment.VariableType.ContainsKey(argument.id))
            {
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.DECL, argument));

            }
            Environment.VariableType.Add(argument.id, (Type)argument.type);
            return null;
        }

        public Type Visit(TypeDeclaration typeDeclaration)
        {
            foreach(var arg in ((FormalList)typeDeclaration.flist).list)
            {
                Environment.VariableType.Add(arg.id, (Type)arg.type);
            }
            if(typeDeclaration.stmt == null)
            {
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.ASN_3, typeDeclaration));
            }

            
            if (((Type)typeDeclaration.type).GetType().Name == "IntType")
            {
                Environment.ReturnType = new IntType();
            }
            else
            {
                Environment.ReturnType = new BoolType();
            }

                typeDeclaration.stmt.Accept(this);

            Environment.ReturnType = null;
            return null;

        }

        public Type Visit(VoidDeclaration voidDeclaration)
        {
            foreach (var arg in ((FormalList)voidDeclaration.flist).list)
            {
                Environment.VariableType.Add(arg.id, (Type)arg.type);
            }
            Environment.ReturnType = null;
            voidDeclaration.stmt.Accept(this);
           
            return null;
        }

        public Type Visit(IfStatement ifStatement)
        {
            var guard = ifStatement.expr.Accept(this);
            var guard_type = guard.GetType().Name;
            if (guard_type != "BoolType")
            {
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.IF, ifStatement));

            }

            ifStatement.stmt.Accept(this);
            
            return null;
        }
    

        public Type Visit(IfElseStatement ifElseStatement)
        {
            var guard = ifElseStatement.expr.Accept(this);
            var guard_type = guard.GetType().Name;
            if (guard_type != "BoolType")
            {
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.IF, ifElseStatement));
            }

            ifElseStatement.stmt1.Accept(this);
            ifElseStatement.stmt2.Accept(this);
            return null;
        }

        public Type Visit(WhileStatement whileStatement)
        {
            var guard = whileStatement.expr.Accept(this);
            var guard_type = guard.GetType().Name;
            if(guard_type != "BoolType")
            {
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.WHILE, whileStatement));

            }

            whileStatement.stmt.Accept(this);
            return null;
        }

        public Type Visit(RegularStatement regularStatement)
        {
            regularStatement.expr.Accept(this);
            return null;
        }

        public Type Visit(Return rreturn)
        {
            if (Environment.ReturnType == null || Environment.ReturnType.GetType() != ((Type)rreturn.e.Accept(this)).GetType())
            {
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.ASN_3, rreturn));
            }
            return null;
        }

        public Type Visit(VoidReturn voidReturn)
        {
            if (Environment.ReturnType != null)
            {
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.ASN_3, voidReturn));

            }
            return null;
        }

        public Type Visit(AssignStatement assignStatement)
        {
            if (!Environment.VariableType.ContainsKey(assignStatement.id))
            {
                
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.ID, assignStatement));

            }
            var type = Environment.VariableType[assignStatement.id];
            var type_text = type.GetType().Name;

            var result = assignStatement.s.Accept(this);
            if(result == null)
            {
                
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.ASN_1, assignStatement));

            }
            var result_type_text = result.GetType().Name;

            if (IsVoidType(result_type_text))
            {
                
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.ASN_1, assignStatement));

            }
            if (result_type_text != type_text)
            {
                
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.ASN_2, assignStatement));

            }

            return type;

        }

        public Type Visit(NotStatement notStatement)
        {
            var type = notStatement.s1.Accept(this);
            var type_text = type.GetType().Name;
            if (IsVoidType(type_text))
            {
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.ASN_2, notStatement));

            }
            if (type_text != "BoolType")
            {
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.UOP_1, notStatement));

            }
            return type;
        }

        public Type Visit(NegativeStatement negativeStatement)
        {
            var type = negativeStatement.s1.Accept(this);
            var type_text = type.GetType().Name;
            if (IsVoidType(type_text))
            {
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.ASN_2, negativeStatement));

            }
            if (type_text != "IntType")
            {
                var err = new TypeErrorMessage();
                throw new TypeException(err.ErrorOutput(TypeErrorMessage.ErrorCode.UOP_2, negativeStatement));

            }
            return type;
        }

        public Type Visit(IntType intType)
        {
            return new IntType();
        }

        public Type Visit(BoolType boolType)
        {
            return new BoolType();
        }

        public Type Visit(BoolStatement boolStatement)
        {
            return new BoolType();
        }

        public Type Visit(Type type)
        {
            return new Type();
        }
    }
}
