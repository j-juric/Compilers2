using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using Lab2;
using Syntax;
namespace Evaluator
{
    public enum ValueType { Number, Void, Bool}

    public interface IValue
    {
        ValueType Type { get; }

        public string ToString();
    }

    public struct NumberValue : IValue
    {
        public ValueType Type { get => ValueType.Number; }
        public int Number;

        public NumberValue(int number)
        {
            Number = number;
        }

        public override string ToString()
        {
            return Number.ToString();
        }

    }

    public struct BoolValue : IValue
    {
        public ValueType Type { get => ValueType.Bool; }
        public bool Bool;

        public BoolValue(bool _bool)
        {
            Bool = _bool;
        }

        public override string ToString()
        {
            return Bool.ToString();
        }
    }

    public struct VoidValue:IValue
    {
        public ValueType Type { get => ValueType.Void; }

        public string ToString(EvaluatorEnvironment environment)
        {
            return null;
        }
    }



    // ----------------------------------------- 

    // --- Evaluator Environment

    public class EvaluatorEnvironment
    {
        public Dictionary<string, Tuple<Declaration,bool>> FunctionDict = new Dictionary<string, Tuple<Declaration, bool>>();
        public Stack<Dictionary<string, IValue>> CallStack = new Stack<Dictionary<string, IValue>>();
        
        public IValue ReturnValue { get; set; }
        public EvaluatorEnvironment()
        {
           
        }

        public void InitEnvironment() 
        {
            this.CallStack.Push(new Dictionary<string, IValue>());
            this.ReturnValue = null;
        }

        public void DeclareFunction(VoidDeclaration function)
        {
            this.FunctionDict.Add(function.id, new Tuple<Declaration,bool>(function,true));
        }

        public void DeclareFunction(TypeDeclaration function)
        {
            this.FunctionDict.Add(function.id, new Tuple<Declaration, bool>(function, false));
        }

        public void CallFunction(Dictionary<string, IValue> frame)
        {
            this.CallStack.Push(frame);
        }

        public void PopFunction()
        {
            this.CallStack.Pop();
        }

        public IValue GetVarValue(string id)
        {
            return this.CallStack.Peek()[id];
        }

        public bool ContainsVarInCurrCall(string id)
        {
            return this.CallStack.Peek().ContainsKey(id);
        }

        public void AddVar(string id, IValue value)
        {
            
            this.CallStack.Peek().Add(id, value);
        }

        public void UpdateVar(string id, IValue value)
        {
            this.CallStack.Peek()[id] = value;
        }
        public Dictionary<string, IValue> GetCurrentFrame()
        {
            return this.CallStack.Peek();
        }

    }

    // --- ERROR HANDLING

    

    public class EvaluationError : Exception
    {
        public EvaluationError(string msg) : base(msg)
        {
        }

    }

    // --- Evaluate Visitor

    public class EvaluatorVisitor : IExpressionVisitor<IValue>
    {

        enum OperatorExpression
        {
            LOGICAL,
            INTEGER,
            EQUALITY,
            INEQUALITY
        }
          
        public EvaluatorEnvironment Environment = new EvaluatorEnvironment();

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

        public IValue Visit(IdentifierStatement identifierStatement)
        {
            //Console.WriteLine("IDENTIFIER STATEMENT");
            if (identifierStatement.list == null)
            {
                
                if (!Environment.ContainsVarInCurrCall(identifierStatement.id))
                {
                    var err = new ErrorMessage();
                    throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.ID, identifierStatement));

                }
                //identifierStatement.id.Accept(this);
                return Environment.GetVarValue(identifierStatement.id);
            }
            else // if it is a function call
            {
               if(identifierStatement.id == "print")
               {
                    //Print
                    foreach (var e in ((ListStatement)identifierStatement.list).exprs)
                    {
                        Console.Write(((IValue)e.Accept(this)).ToString() + " ");
                    }
                    Console.Write("\n");
                    return null;
               }
               else
               {
                    if(!Environment.FunctionDict.ContainsKey(identifierStatement.id))
                    {
                        var err = new ErrorMessage();
                        throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.CALL_1, identifierStatement));

                    }
                    else
                    {
                        var info = Environment.FunctionDict[identifierStatement.id];
                        if (info.Item2 == true) // If void function
                        {
                            var function = (VoidDeclaration)Environment.FunctionDict[identifierStatement.id].Item1;
                            var numOfArg = (function.flist == null) ? 0 : ((FormalList)function.flist).list.Count;
                            if (((ListStatement)identifierStatement.list).exprs.Count != numOfArg)
                            {
                                var err = new ErrorMessage();
                                throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.CALL_2, identifierStatement));

                            }
                            else
                            {
                                var parameters = new Dictionary<string, IValue>();
                                for (int i = 0; i < ((ListStatement)identifierStatement.list).exprs.Count; i++){
                                    parameters.Add(  ((FormalList)function.flist)  .list[i].id, ((ListStatement)identifierStatement.list).exprs[i].Accept(this));
                                }
                                Environment.CallFunction(parameters);
                                function.Accept(this);
                                //ovde treba pop value
                                Environment.PopFunction();
                                var result = Environment.ReturnValue;
                                Environment.ReturnValue = null;                                
                                return result;

                            }
                        }
                        else // if type function
                        {
                            var function = (TypeDeclaration)Environment.FunctionDict[identifierStatement.id].Item1;
                            var numOfArg = (function.flist == null) ? 0 : ((FormalList)function.flist).list.Count;
                            if (((ListStatement)identifierStatement.list).exprs.Count != numOfArg)
                            {
                                var err = new ErrorMessage();
                                throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.CALL_2, identifierStatement));

                            }
                            else
                            {
                                var parameters = new Dictionary<string, IValue>();
                                for (int i = 0; i < ((ListStatement)identifierStatement.list).exprs.Count; i++)
                                {
                                   
                                    parameters.Add(((FormalList)function.flist).list[i].id, ((ListStatement)identifierStatement.list).exprs[i].Accept(this));
                                }
                                
                                Environment.CallFunction(parameters);
                                
                                function.Accept(this);
                                //ovde treba pop value
                                Environment.PopFunction();
                                var result = Environment.ReturnValue;
                                Environment.ReturnValue = null;
                                var funcType = (function.type.GetType().Name == "IntType") ? ValueType.Number : ValueType.Bool;
                                if(result == null || result.Type != funcType)
                                {
                                    var err = new ErrorMessage();
                                    throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.CALL_3, identifierStatement));
                                }
                                return result;

                            }
                        }
                        
                    }
                }
            }
            

        }

        // ---

        public IValue Visit(BinOperatorStatement binaryOperatorExpression)
        {
            var left = binaryOperatorExpression.left.Accept(this);
            var right = binaryOperatorExpression.right.Accept(this);

            var opType = MapOpExpr[binaryOperatorExpression.type];
           
            
            if (opType == OperatorExpression.INTEGER)
            {
                if (left.Type == ValueType.Number && right.Type == ValueType.Number)
                {
                    var x = ((NumberValue)left).Number;
                    var y = ((NumberValue)right).Number;
                    switch (binaryOperatorExpression.type)
                    {
                        case BinOperatorStatement.Type.ADD:
                            return new NumberValue(x + y);
                        case BinOperatorStatement.Type.SUB:
                            return new NumberValue(x - y);
                        case BinOperatorStatement.Type.MUL:
                            return new NumberValue(x * y);
                        case BinOperatorStatement.Type.DIV:
                            return new NumberValue(x / y);
                    }
                }
                else
                {
                    var err = new ErrorMessage();
                    throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.BOP_1, binaryOperatorExpression));
                }
            }
            else if (opType == OperatorExpression.LOGICAL) 
            {
                if (left.Type == ValueType.Bool && right.Type == ValueType.Bool)
                {
                    var x = ((BoolValue)left).Bool;
                    var y = ((BoolValue)right).Bool;
                    switch (binaryOperatorExpression.type)
                    {
                        case BinOperatorStatement.Type.AND:
                            return new BoolValue(x && y);
                        case BinOperatorStatement.Type.OR:
                            return new BoolValue(x || y);
                    }
                }
                else
                {
                    var err = new ErrorMessage();
                    ErrorMessage.ErrorCode code = (binaryOperatorExpression.type == BinOperatorStatement.Type.OR) ? ErrorMessage.ErrorCode.BOP_2 : ErrorMessage.ErrorCode.BOP_3;
                    throw new EvaluationError(err.ErrorOutput(code, binaryOperatorExpression));
                }
            }
            else if(opType == OperatorExpression.EQUALITY)
            {
                if (left.Type == ValueType.Number && right.Type == ValueType.Number)
                {
                    var x = ((NumberValue)left).Number;
                    var y = ((NumberValue)right).Number;
                    switch (binaryOperatorExpression.type)
                    {
                        case BinOperatorStatement.Type.EQ:
                            return new BoolValue(x == y);
                        case BinOperatorStatement.Type.NEQ:
                            return new BoolValue(x != y);
                    }
                }
                else if(left.Type == ValueType.Bool && right.Type == ValueType.Bool)
                {
                    var x = ((BoolValue)left).Bool;
                    var y = ((BoolValue)right).Bool;
                    switch (binaryOperatorExpression.type)
                    {
                        case BinOperatorStatement.Type.EQ:
                            return new BoolValue(x == y);
                        case BinOperatorStatement.Type.NEQ:
                            return new BoolValue(x != y);
                    }
                }
                else
                {
                    
                    var err = new ErrorMessage();
                    throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.BOP_4, binaryOperatorExpression));
                }
            }
            else if(opType == OperatorExpression.INEQUALITY)
            {
                if (left.Type == ValueType.Number && right.Type == ValueType.Number)
                {
                    var x = ((NumberValue)left).Number;
                    var y = ((NumberValue)right).Number;
                    
                    switch (binaryOperatorExpression.type)
                    {
                        case BinOperatorStatement.Type.GR:
                            return new BoolValue(x > y);
                        case BinOperatorStatement.Type.LE:
                            return new BoolValue(x < y);
                        case BinOperatorStatement.Type.GEQ:
                            return new BoolValue(x >= y);  
                        case BinOperatorStatement.Type.LEQ:
                            return new BoolValue(x <= y);
                    }
                }
                else
                {
                    var err = new ErrorMessage();
                    throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.BOP_5, binaryOperatorExpression));
                }
            }
            else
            {
                return null;
                //error
            }

            return null;
        }

        // ---      

        public IValue Visit(IfStatement ifStatement)
        {
            var guard = ifStatement.expr.Accept(this);
            if (guard.Type == ValueType.Bool)
            {
                if (((BoolValue)guard).Bool == true)
                {
                    return ifStatement.stmt.Accept(this);
                }
                return null;

            }
            else
            {
                var err = new ErrorMessage();
                throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.IF, ifStatement));
            }

        }
        public IValue Visit(IfElseStatement ifElseStatement)
        {
            var guard = ifElseStatement.expr.Accept(this);

            if (guard.Type == ValueType.Bool)
            {
                if (((BoolValue)guard).Bool == true)
                {
                    return ifElseStatement.stmt1.Accept(this);
                }
                else
                {
                    return ifElseStatement.stmt2.Accept(this);
                }

            }

            var err = new ErrorMessage();
            throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.IF, ifElseStatement));
        }

        public IValue Visit(WhileStatement whileStatement)
        {
            var guard = whileStatement.expr.Accept(this);

            if (guard.Type == ValueType.Bool)
            {
                while (((BoolValue)guard).Bool != false)
                {
                    whileStatement.stmt.Accept(this);
                    guard = whileStatement.expr.Accept(this);
                }
                return null;
            }

            else
            {
                var err = new ErrorMessage();
                throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.WHILE, whileStatement));
            }
        }

        // ---

        /// /////////////////////////////////////////////////////////////////////////////////////////////////



        public IValue Visit(DeclarationSequence declarationSequence)
        {
            //Console.WriteLine("DECLARATION SEQUENCE STATEMENT");
            var declaration = declarationSequence;
            while(declaration != null)
            {
                var typeName = declaration.tail.GetType().Name;

                if (typeName == "VoidDeclaration") 
                    Environment.DeclareFunction((VoidDeclaration)declaration.tail);
                else
                    Environment.DeclareFunction((TypeDeclaration)declaration.tail);
                declaration = (DeclarationSequence)declaration.head;

            }
            if (Environment.FunctionDict.ContainsKey("main"))
            {
                //Console.WriteLine("Main exists");
                
                var main = Environment.FunctionDict["main"].Item1;
                var typeName = main.GetType().Name;
                if (typeName== "VoidDeclaration")
                    main = (VoidDeclaration)Environment.FunctionDict["main"].Item1;
                else
                    main = (TypeDeclaration)Environment.FunctionDict["main"].Item1;
         
                Environment.InitEnvironment();
                main.Accept(this);
                Environment.PopFunction();
                return null;
            }
            else
            {
                //BACI GRESKU
                return null;
            }
        }

        public IValue Visit(SequenceStatement sequenceStatement)
        {
            //Console.WriteLine("SEQUENCE STATEMENT");
            if (sequenceStatement.head != null)
            {
                if (Environment.ReturnValue != null)
                    return null;
                ((SequenceStatement)sequenceStatement.head).Accept(this);
            }
            if (Environment.ReturnValue != null)
                return null;
            //Console.WriteLine("SEQUENCE STATEMENT TAIL");
            sequenceStatement.tail.Accept(this);

            return null;
        }

        public IValue Visit(NumStatement numStatement)
        {
            
            return new NumberValue(numStatement.num);
        }

        public IValue Visit(ListStatement listStatement)
        {
            //var list = listStatement.Accept(this);
            //var x = ((ListValue)list).List;
            //foreach (var s in listStatement.exprs)
            //{
            //    x.Add(s.Accept(this));
            //}

            //return new ListValue(x);

            return null;

        }  

        public IValue Visit(FormalList formalList)
        {
            // return formalList.Accept(this);
            return null;
        }

        public IValue Visit(Declaration declaration)
        {
            return null;
        }

        public IValue Visit(Argument argument)
        {
            //var current = Environment.GetCurrentFrame();
            //Console.WriteLine("ARGUMENT STATEMENT");
            if (Environment.ContainsVarInCurrCall(argument.id))
            {
                var err = new ErrorMessage();
                throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.DECL, argument));

            }
            
            var typeName = ((Syntax.Type)argument.type).GetType().Name;
            IValue value = new NumberValue(0);
            if (typeName != "IntType") value = new BoolValue(false);
            Environment.AddVar(argument.id, value);

            return null; // new PointerValue(0);
        }
        public IValue Visit(TypeDeclaration typeDeclaration)
        {
            //Console.WriteLine("TypeFunc STATEMENT");
            if (typeDeclaration.stmt!=null)typeDeclaration.stmt.Accept(this);
            else
            {
                var err = new ErrorMessage();
                throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.CALL_3, typeDeclaration));
            }
            return null;// pointer;
        }
        public IValue Visit(VoidDeclaration voidDeclaration)
        {
            //Console.WriteLine("VoidFunc STATEMENT");
            voidDeclaration.stmt.Accept(this);
            return null;
        }
        public IValue Visit(BlockStatement blockStatement)
        {
            //Console.WriteLine("Block Statement");
            blockStatement.stmt.Accept(this);
            return null;
        }
        public IValue Visit(RegularStatement regularStatement)
        {
            //Console.WriteLine("Regular Statement");
            regularStatement.expr.Accept(this);
            return null;
        }
        public IValue Visit(Return rreturn)
        {
            Environment.ReturnValue = rreturn.e.Accept(this);
            return null;
        }
        public IValue Visit(VoidReturn voidReturn)
        {
            Environment.ReturnValue = new VoidValue();
            return null;
        }
        public IValue Visit(AssignStatement assignStatement)
        {
            //Console.WriteLine("ASSIGN STATEMENT");
            if (!Environment.ContainsVarInCurrCall(assignStatement.id))
            {
                var err = new ErrorMessage();
                throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.ID, assignStatement));

            }
            IValue result = assignStatement.s.Accept(this);
            if(result.Type == null)
            {
                var err = new ErrorMessage();
                throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.ASN_1, assignStatement));

            }
            if(Environment.GetVarValue(assignStatement.id).Type != result.Type)
            {
                var err = new ErrorMessage();
                throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.ASN_2, assignStatement));

            }
            Environment.UpdateVar(assignStatement.id, result);
            return result;
        }
        public IValue Visit(NotStatement notStatement)
        {
            IValue result = notStatement.s1.Accept(this);
            if (result.Type != ValueType.Bool)
            {
                var err = new ErrorMessage();
                throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.UOP_1, notStatement));

            }
            var x = ((BoolValue)result).Bool;

            return new BoolValue(!x);
        }
        public IValue Visit(NegativeStatement negativeStatement)
        {
            var result = negativeStatement.s1.Accept(this);
            var x = ((NumberValue)result).Number;
            if (result.Type != ValueType.Number)
            {
                var err = new ErrorMessage();
                throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.UOP_1, negativeStatement));

            }

            return new NumberValue(0 - x);
        }
        public IValue Visit(BoolStatement boolStatement)
        {
            
            return new BoolValue (boolStatement.value);// new PointerValue(0); ///??????????????????????????????
        }
        public IValue Visit(Syntax.Type type)
        {
            return null;// new PointerValue(0); ////??????????????????????????
        }
        public IValue Visit(IntType intType)
        {
            return null;
        }
        public IValue Visit(BoolType boolType)
        {
            return null;
        }
    }

}
