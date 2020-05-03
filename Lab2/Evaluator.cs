using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using Syntax;
namespace Evaluator
{
    public enum ValueType { Number, Pointer, Bool , List}

    public interface IValue
    {
        ValueType Type { get; }

        string ToString(EvaluatorEnvironment environment);
    }

    public struct NumberValue : IValue
    {
        public ValueType Type { get => ValueType.Number; }
        public int Number;

        public NumberValue(int number)
        {
            Number = number;
        }

        public string ToString(EvaluatorEnvironment environment)
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

        public string ToString(EvaluatorEnvironment environment)
        {
            return Bool.ToString();
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

    public class ErrorMessage
    {
        public enum ErrorCode
        {
            UOP_1, // Expected value in unary - operator
            UOP_2, // Expected boolean in unary ! operator
            BOP_1, // Expected integers in arithmetic binary operator
            BOP_2, // Expected booleans in || operator
            BOP_3, // Expected booleans in && operator
            BOP_4, // Expected same value type in binary equality operators
            BOP_5, // Expected integers in binary inequality operators
            ID, // Variable not declared
            ASN, // Expected value in assignment
            CALL_1, // Function not defined
            CALL_2, // Wrong number of arguments in function call
            DECL, // Variable already defined
            IF, // Expecting boolean guard in if
            WHILE, // Expecting boolean guard in while
        }
        Dictionary<ErrorCode, string> errorDescription = new Dictionary<ErrorCode, string>()
        {
            { ErrorCode.UOP_1 , "Expected value in unary - operator" },
            { ErrorCode.UOP_2 , "Expected boolean in unary ! operator" },
            { ErrorCode.BOP_1 , "Expected integers in arithmetic binary operator" },
            { ErrorCode.BOP_2 , "Expected booleans in || operator" },
            { ErrorCode.BOP_3 , "Expected booleans in && operator" },
            { ErrorCode.BOP_4 , "Expected same value type in binary equality operators" },
            { ErrorCode.BOP_5 , "Expected integers in binary inequality operators" },
            { ErrorCode.ID , "Expected value in unary - operator" },
            { ErrorCode.ASN , "Expected value in unary - operator" },
            { ErrorCode.CALL_1 , "Expected value in unary - operator" },
            { ErrorCode.CALL_2 , "Expected value in unary - operator" },
            { ErrorCode.DECL , "Expected value in unary - operator" },
            { ErrorCode.IF , "Expected value in unary - operator" },
            { ErrorCode.WHILE , "Expected value in unary - operator" },
        };
        public string ErrorOutput(ErrorCode ec, Locatable loc)
        {
            return $"Error ({ec.ToString()}) - Line :{loc.line.ToString()}, Column:{loc.column.ToString()} | {errorDescription[ec]}";
        }
    }

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
            if (identifierStatement.list == null)
            {
                
                if (!Environment.ContainsVarInCurrCall(identifierStatement.id))
                {
                    var err = new ErrorMessage();
                    throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.ID, identifierStatement));

                }
                identifierStatement.Accept(this);
                return Environment.GetVarValue(identifierStatement.id);
            }
            else // if it is a function call
            {
               if(identifierStatement.id == "print")
               {
                    //Print
                    foreach (var e in ((ListStatement)identifierStatement.list).exprs)
                    {
                        Console.Write(e.Accept(this) + " ");
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
                                return Environment.ReturnValue;

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
                                return Environment.ReturnValue;

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
                        case BinOperatorStatement.Type.AND:
                            return new BoolValue(x == y);
                        case BinOperatorStatement.Type.OR:
                            return new BoolValue(x != y);
                    }
                }
                else if(left.Type == ValueType.Bool && right.Type == ValueType.Bool)
                {
                    var x = ((BoolValue)left).Bool;
                    var y = ((BoolValue)right).Bool;
                    switch (binaryOperatorExpression.type)
                    {
                        case BinOperatorStatement.Type.AND:
                            return new BoolValue(x == y);
                        case BinOperatorStatement.Type.OR:
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

            }
            var err = new ErrorMessage();
            throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.IF,ifStatement));
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
            var guard = whileStatement.Accept(this);

            if (guard.Type == ValueType.Bool)
            {
                while (((BoolValue)guard).Bool != false)
                {
                    whileStatement.stmt.Accept(this);
                    guard = whileStatement.Accept(this);
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
            if (declarationSequence.head != null) 
                declarationSequence.head.Accept(this);
            return declarationSequence.tail.Accept(this);
        }

        public IValue Visit(SequenceStatement sequenceStatement)
        {
            if (sequenceStatement.head != null) sequenceStatement.head.Accept(this);
            return sequenceStatement.tail.Accept(this);
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

        }  //valjda je dobro...

        public IValue Visit(FormalList formalList)
        {
            return formalList.Accept(this);
        }

        public IValue Visit(Declaration declaration)
        {
            return null;
        }

        public IValue Visit(Argument argument)
        {
            //var current = Environment.GetCurrentFrame();
            if (Environment.ContainsVarInCurrCall(argument.id))
            {
                var err = new ErrorMessage();
                throw new EvaluationError(err.ErrorOutput(ErrorMessage.ErrorCode.DECL, argument));

            }
            typeof(argument.type)
            Environment.AddVar(argument.id, (argument.type))
            //var pointer = Environment.Allocate();
            //current[argument.id] = pointer;
            return null; // new PointerValue(0);
        }

        public IValue Visit(TypeDeclaration typeDeclaration)
        {
            var current = Environment.CallStack.Peek();
            var context = new Dictionary<string, IValue>();
            var free = new SortedSet<string>();
            typeDeclaration.Accept(FreeVariablesVisitor.Instance, free);

            foreach (var identifier in free)
            {
                context[identifier] = current[identifier];
            }

            //var pointer = null; // Environment.Allocate();
            //Environment.Store[pointer.Pointer] = new HeapClosure(context, typeDeclaration.id, typeDeclaration.stmt);
            return null;// pointer;
        }

        public IValue Visit(VoidDeclaration voidDeclaration)
        {
            throw new NotImplementedException();
        }


        public IValue Visit(BlockStatement blockStatement)
        {
            throw new NotImplementedException();
        }
        public IValue Visit(RegularStatement regularStatement)
        {
            throw new NotImplementedException();
        }
        public IValue Visit(Return rreturn)
        {
            throw new NotImplementedException();
        }
        public IValue Visit(VoidReturn voidReturn)
        {
            throw new NotImplementedException();
        }
        public IValue Visit(AssignStatement assignStatement)
        {
            throw new NotImplementedException();
        }


        public IValue Visit(NotStatement notStatement)
        {
            var s = notStatement.s1.Accept(this);
            var x = ((BoolValue)s).Bool;

            return new BoolValue(!x);
        }

        public IValue Visit(NegativeStatement negativeStatement)
        {
            var s = negativeStatement.s1.Accept(this);
            var x = ((NumberValue)s).Number;

            return new NumberValue(0 - x);
        }



        public IValue Visit(BoolStatement boolStatement)
        {
            return null;// new PointerValue(0); ///??????????????????????????????
        }


        public IValue Visit(Syntax.Type type)
        {
            return null;// new PointerValue(0); ////??????????????????????????
        }

        public IValue Visit(IntType intType)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(BoolType boolType)
        {
            throw new NotImplementedException();
        }
    }

}
