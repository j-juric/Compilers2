using System;
using System.Collections.Generic;
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

    public struct ListValue : IValue
    {
        public ValueType Type { get => ValueType.List; }
        public List<IValue> List;

        public ListValue(List<IValue> _list)
        {
            List = _list;
        }

        public string ToString(EvaluatorEnvironment environment)
        {
            return List.ToString();
        }
    }

    public struct PointerValue : IValue
    {
        public ValueType Type { get => ValueType.Pointer; }
        public int Pointer;

        public PointerValue(int pointer)
        {
            Pointer = pointer;
        }

        public string ToString(EvaluatorEnvironment environment)
        {
            var value = environment.Store[Pointer];
            return value.ToString(environment);
        }
    }

    // -----------------------------------------

    public enum HeapItemType { Tuple, Cons, Closure, NativeClosure};

    public interface IHeapItem
    {
        HeapItemType Type { get; }

        string ToString(EvaluatorEnvironment environment);
    }

    public struct HeapTuple : IHeapItem
    {
        public HeapItemType Type { get => HeapItemType.Tuple; }
        public IValue First;
        public IValue Second;

        public HeapTuple(IValue first, IValue second)
        {
            First = first;
            Second = second;
        }

        public string ToString(EvaluatorEnvironment environment)
        {
            var first = First.ToString(environment);
            var second = Second.ToString(environment);
            return $"({first},{second})";
        }

    }

    public struct HeapCons : IHeapItem
    {
        public HeapItemType Type { get => HeapItemType.Cons; }
        public IValue Head;
        public PointerValue Tail;

        public HeapCons(IValue head, PointerValue tail)
        {
            Head = head;
            Tail = tail;
        }

        public string ToString(EvaluatorEnvironment environment)
        {
            var head = Head.ToString(environment);
            var tail = Tail.ToString(environment);
            return $"[{head},{tail[1..^1]}]";
        }
    }

    public struct HeapClosure : IHeapItem
    {
        public HeapItemType Type { get => HeapItemType.Closure; }
        public Dictionary<string, IValue> Context;
        public string Id;
        public Statement Body;

        public HeapClosure(Dictionary<string, IValue> context, string id, Statement body)
        {
            Context = context;
            Id = id;
            Body = body;
        }

        public string ToString(EvaluatorEnvironment environment)
        {
            return "<closure>";
        }

    }

    public struct NativeClosure : IHeapItem
    {
        public HeapItemType Type { get => HeapItemType.NativeClosure; }
        //public Dictionary<List<IValue>, string> Native;  
        public bool Contains(string function)
        {
            bool flag = false;
            switch (function)
            {
                case "print":
                    flag = true;
                    break;
            }
            return flag;
        }

        public void callNativeFunction(List<IValue> args,String nativeFun)
        {
            switch (nativeFun)
            {
                case "print":
                    {
                        if (args.Count != 0)
                        {
                            //Console.Write(" ");
                            foreach (var a in args)
                            {
                                Console.Write(a.ToString());
                                Console.Write(" ");
                            }
                            Console.Write(Environment.NewLine);
                        }
                        break;
                    }
                    
            }
        }
        
        public string ToString(EvaluatorEnvironment environment)
        {
            return "<native_closure>";
        }
    }

    // ---

    public class EvaluatorEnvironment
    {
        public Dictionary<int, IHeapItem> Store = new Dictionary<int, IHeapItem>();
        public Stack<Dictionary<string, IValue>> CallStack = new Stack<Dictionary<string, IValue>>();
        public NativeClosure NativeFunc;

        int Free = 1;

        public EvaluatorEnvironment()
        {
            CallStack.Push(new Dictionary<string, IValue>());
        }

        public PointerValue Allocate()
        {
            var pointer = Free++;
            return new PointerValue(pointer);
        }



    }

    // ---

    public class EvaluationError : Exception
    {
        public EvaluationError(string msg) : base(msg)
        {
        }
    }

    // ---
    /*
    public class EvaluatorVisitor : IExpressionVisitor<IValue>
    {
        public EvaluatorEnvironment Environment = new EvaluatorEnvironment();


        public IValue Visit(IdentifierStatement identifierStatement)
        {
            if (identifierStatement.list == null)
            {
                var current = Environment.CallStack.Peek();
                if (!current.ContainsKey(identifierStatement.id))
                {
                    throw new EvaluationError($"Undefined identifier {identifierStatement.id}");
                }
                return current[identifierStatement.id];
            }
            else
            {
                if (Environment.NativeFunc.Contains(identifierStatement.id))
                {
                    List<IValue> args = new List<IValue>();
                    Environment.NativeFunc.callNativeFunction(args,identifierStatement.id);
                    foreach(var a in identifierStatement.list)
                    {

                    }
                }
                else
                {
                    var current = Environment.CallStack.Peek();
                    var context = new Dictionary<string, IValue>();
                    var free = new SortedSet<string>();
                    identifierStatement.Accept(FreeVariablesVisitor.Instance, free);
                    var list = identifierStatement.list.Accept(this);

                    foreach (var identifier in free)
                    {
                        context[identifier] = current[identifier];
                    }

                    var pointer = Environment.Allocate();
                    Environment.Store[pointer.Pointer] = new HeapClosure(context, lambdaExpression.Id, lambdaExpression.Body);
                    return pointer;
                }
            }
            return null; // da ne smara
            
        }

        // ---

        public IValue Visit(BinOperatorStatement binaryOperatorExpression)
        {
            var left = binaryOperatorExpression.left.Accept(this);
            var right = binaryOperatorExpression.right.Accept(this);

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

            throw new EvaluationError($"Addition expects numbers");
        }

        // ---      

        public IValue Visit(IfStatement ifStatement)
        {
            var guard = ifStatement.expr.Accept(this);
            if (guard.Type == ValueType.Number)
            {
                if (((NumberValue)guard).Number != 0)
                {
                    return ifStatement.stmt.Accept(this);
                }
                return null;
                //else
                //{
                //    return ifStatement.Else.Accept(this);
                //}
            }
            if (guard.Type == ValueType.Bool)
            {
                if (((BoolValue)guard).Bool == true)
                {
                    return ifStatement.stmt.Accept(this);
                }
                return null;
                //else
                //{
                //    return ifStatement.Else.Accept(this);
                //}
            }

            throw new EvaluationError("if expected number or bool guard");

        }

        // ---

        public IValue Visit(WhileStatement whileStatement)
        {
            var guard = whileStatement.Accept(this);
            if(guard.Type== ValueType.Number)
                while (((NumberValue)guard).Number != 0)
                {
                    whileStatement.stmt.Accept(this);
                    guard = whileStatement.Accept(this);
                }
            if (guard.Type == ValueType.Bool)
                while ( ((BoolValue)guard).Bool != false)
                {
                    whileStatement.stmt.Accept(this);
                    guard = whileStatement.Accept(this);
                }

            return new PointerValue(0);
        }

        // ---

        /// /////////////////////////////////////////////////////////////////////////////////////////////////



        public IValue Visit(DeclarationSequence declarationSequence)
        {
            if(declarationSequence.head!=null) declarationSequence.head.Accept(this);
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

        public IValue Visit(BracketStatement bracketStatement)
        {
            return bracketStatement.stmt.Accept(this);
        }

        public IValue Visit(ListStatement listStatement)
        {
            var list = listStatement.Accept(this);
            var x = ((ListValue)list).List;
            foreach(var s in listStatement.exprs)
            {
                x.Add(s.Accept(this));
            }

            return new ListValue(x);
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
            var current = Environment.CallStack.Peek();
            if (current.ContainsKey(argument.id))
            {
                throw new EvaluationError($"Identifier already defined {argument.id}");
            }
            var pointer = Environment.Allocate();
            current[argument.id] = pointer;
            return new PointerValue(0);
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

            var pointer = Environment.Allocate();
            Environment.Store[pointer.Pointer] = new HeapClosure(context, typeDeclaration.id, typeDeclaration.stmt);
            return pointer;
        }

        public IValue Visit(VoidDeclaration voidDeclaration)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(IfElseStatement ifElseStatement)
        {
            var guard = ifElseStatement.expr.Accept(this);
            if (guard.Type == ValueType.Number)
            {
                if (((NumberValue)guard).Number != 0)
                {
                    return ifElseStatement.stmt1.Accept(this);
                }
                else
                {
                    return ifElseStatement.stmt2.Accept(this);
                }
                
            }
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

            throw new EvaluationError("if else expected number or bool guard");
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

        public IValue Visit(OrStatement orStatement)
        {
            var left = orStatement.s1.Accept(this);
            var right = orStatement.s2.Accept(this);

            var x = ((BoolValue)left).Bool;
            var y = ((BoolValue)right).Bool;


            return new BoolValue(x || y);
        }     

        public IValue Visit(AndStatement andStatement)
        {
            var left = andStatement.s1.Accept(this);
            var right = andStatement.s2.Accept(this);

            var x = ((BoolValue)left).Bool;
            var y = ((BoolValue)right).Bool;


            return new BoolValue(x && y);
        }

        public IValue Visit(NotEqStatement notEqStatement)
        {
            var left = notEqStatement.s1.Accept(this);
            var right = notEqStatement.s2.Accept(this);

            var x = ((BoolValue)left).Bool;
            var y = ((BoolValue)right).Bool;


            return new BoolValue(x != y);
        }

        public IValue Visit(EqStatement eqStatement)
        {
            var left = eqStatement.s1.Accept(this);
            var right = eqStatement.s2.Accept(this);

            var x = ((BoolValue)left).Bool;
            var y = ((BoolValue)right).Bool;


            return new BoolValue(x == y);
        }
        public IValue Visit(LesserStatement lesserStatement)
        {
            var left = lesserStatement.s1.Accept(this);
            var right = lesserStatement.s2.Accept(this);

            var x = ((NumberValue)left).Number;
            var y = ((NumberValue)right).Number;


            return new BoolValue(x < y);
        }

        public IValue Visit(GreaterStatement greaterStatement)
        {
            var left = greaterStatement.s1.Accept(this);
            var right = greaterStatement.s2.Accept(this);

            var x = ((NumberValue)left).Number;
            var y = ((NumberValue)right).Number;


            return new BoolValue(x > y);
        }

        public IValue Visit(LEqStatement leqStatement)
        {
            var left = leqStatement.s1.Accept(this);
            var right = leqStatement.s2.Accept(this);

            var x = ((NumberValue)left).Number;
            var y = ((NumberValue)right).Number;


            return new BoolValue(x <= y);
        }

        public IValue Visit(GEqStatement geqStatement)
        {
            var left = geqStatement.s1.Accept(this);
            var right = geqStatement.s2.Accept(this);

            var x = ((NumberValue)left).Number;
            var y = ((NumberValue)right).Number;


            return new BoolValue(x >= y);
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

            return new NumberValue(0-x);
        }

        public IValue Visit(IntStatement intStatement)
        {
            return new PointerValue(0); ///??????????????????????????????
        }

        public IValue Visit(BoolStatement boolStatement)
        {
             return new PointerValue(0); ///??????????????????????????????
        }

        public IValue Visit(TrueStatement trueStatement)
        {
            return new BoolValue(true);
        }

        public IValue Visit(FalseStatement falseStatement)
        {
            return new BoolValue(false);
        }

        public IValue Visit(Syntax.Type type)
        {
            return new PointerValue(0); ////??????????????????????????
        }
    }
    */
}
