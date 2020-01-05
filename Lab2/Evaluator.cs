using System;
using System.Collections.Generic;
using Syntax;
namespace Evaluator
{
    public enum ValueType { Number, Pointer, Bool }

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

    // ---

    public enum HeapItemType { Tuple, Cons, Closure };

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

    // ---

    public class EvaluatorEnvironment
    {
        public Dictionary<int, IHeapItem> Store = new Dictionary<int, IHeapItem>();
        public Stack<Dictionary<string, IValue>> CallStack = new Stack<Dictionary<string, IValue>>();

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

    public class EvaluatorVisitor : IExpressionVisitor<IValue>
    {
        public EvaluatorEnvironment Environment = new EvaluatorEnvironment();


        public IValue Visit(IdentifierStatement identifierStatement)
        {
            var current = Environment.CallStack.Peek();
            if (!current.ContainsKey(identifierStatement.id))
            {
                throw new EvaluationError($"Undefined identifier {identifierStatement.id}");
            }
            return current[identifierStatement.id];
        }

        // ---

        public IValue Visit(NumberExpression numberExpression)
        {
            return new NumberValue(numberExpression.Num);
        }

        // ---

        public IValue Visit(BinaryOperatorExpression binaryOperatorExpression)
        {
            var left = binaryOperatorExpression.Left.Accept(this);
            var right = binaryOperatorExpression.Right.Accept(this);

            if (left.Type == ValueType.Number && right.Type == ValueType.Number)
            {
                var x = ((NumberValue)left).Number;
                var y = ((NumberValue)right).Number;

                switch (binaryOperatorExpression.Type)
                {
                    case BinaryOperatorExpression.OperatorType.Add:
                        return new NumberValue(x + y);
                    case BinaryOperatorExpression.OperatorType.Sub:
                        return new NumberValue(x - y);
                    case BinaryOperatorExpression.OperatorType.Mul:
                        return new NumberValue(x * y);
                    case BinaryOperatorExpression.OperatorType.Div:
                        return new NumberValue(x / y);
                    case BinaryOperatorExpression.OperatorType.Eq:
                        return new NumberValue(x == y ? 1 : 0);
                    case BinaryOperatorExpression.OperatorType.NEq:
                        return new NumberValue(x == y ? 0 : 1);
                }
            }

            throw new EvaluationError($"Addition expects numbers");
        }

        // ---

        public IValue Visit(LetExpression letExpression)
        {
            // we're implementing let rec, make a pre-closure if body is a lamb
            var pointer = Environment.Allocate();
            var current = Environment.CallStack.Peek();
            current[letExpression.Id] = pointer;

            var value = letExpression.Expr1.Accept(this);

            if (value.Type == ValueType.Pointer)
            {
                var closure = Environment.Store[((PointerValue)value).Pointer];
                Environment.Store[pointer.Pointer] = closure;
            }
            else
            {
                current[letExpression.Id] = value;
            }

            var result = letExpression.Expr2.Accept(this);
            current.Remove(letExpression.Id);
            return result;
        }

        // ---

        public IValue Visit(IfExpression ifExpression)
        {
            var guard = ifExpression.Guard.Accept(this);
            if (guard.Type == ValueType.Number)
            {
                if (((NumberValue)guard).Number != 0)
                {
                    return ifExpression.Then.Accept(this);
                }
                else
                {
                    return ifExpression.Else.Accept(this);
                }
            }

            throw new EvaluationError("if expected number guard");

        }

        // ---

        public IValue Visit(WhileExpression whileExpression)
        {
            var guard = whileExpression.Accept(this);
            while (guard.Type == ValueType.Number && (((NumberValue)guard).Number != 0))
            {
                whileExpression.Body.Accept(this);
                guard = whileExpression.Accept(this);
            }

            // used as empty tuple, and nil
            return new PointerValue(0);
        }

        // ---

        public IValue Visit(LambdaExpression lambdaExpression)
        {
            var current = Environment.CallStack.Peek();
            var context = new Dictionary<string, IValue>();
            var free = new SortedSet<string>();
            lambdaExpression.Accept(FreeVariablesVisitor.Instance, free);

            foreach (var identifier in free)
            {
                context[identifier] = current[identifier];
            }

            var pointer = Environment.Allocate();
            Environment.Store[pointer.Pointer] = new HeapClosure(context, lambdaExpression.Id, lambdaExpression.Body);
            return pointer;
        }

        // ---

        public IValue Visit(ApplicationExpression applicationExpression)
        {
            var function = applicationExpression.Function.Accept(this);
            var argument = applicationExpression.Argument.Accept(this);

            if (function.Type == ValueType.Pointer)
            {
                var pointer = ((PointerValue)function).Pointer;
                var item = Environment.Store[pointer];
                if (item.Type == HeapItemType.Closure)
                {
                    var closure = (HeapClosure)item;
                    // we must clone the context - otherwise this call affects other calls
                    var context = new Dictionary<string, IValue>(closure.Context)
                    {
                        [closure.Id] = argument
                    };
                    Environment.CallStack.Push(context);
                    var result = closure.Body.Accept(this);
                    Environment.CallStack.Pop();
                    return result;
                }
            }

            throw new EvaluationError("Application expected closure");
        }


        public IValue Visit(ListExpression listExpression)
        {
            throw new NotImplementedException();
        }


        /// /////////////////////////////////////////////////////////////////////////////////////////////////



        public IValue Visit(DeclarationSequence declarationSequence)
        {
            declarationSequence.head.Accept(this);
            return declarationSequence.tail.Accept(this);
        }

        public IValue Visit(SequenceStatement sequenceStatement)
        {
            sequenceStatement.head.Accept(this);
            return sequenceStatement.tail.Accept(this);
        }

        public IValue Visit(IdentifierStatement identifierStatement)
        {
            var current = Environment.CallStack.Peek();
            if (!current.ContainsKey(identifierStatement.id))
            {
                throw new EvaluationError($"Undefined identifier {identifierStatement.id}");
            }
            return current[identifierStatement.id];
        }

        public IValue Visit(NumStatement numStatement)
        {
            return new NumberValue(numStatement.num);
        }

        public IValue Visit(BracketStatement bracketStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(BinOperatorStatement binOperatorStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(ListStatement listStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(FormalList formalList)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(Declaration declaration)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(Argument argument)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(TypeDeclaration typeDeclaration)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(VoidDeclaration voidDeclaration)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(IfStatement ifStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(IfElseStatement ifElseStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(WhileStatement whileStatement)
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

        public IValue Visit(OrStatement orStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(AndStatement andStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(NotEqStatement notEqStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(EqStatement eqStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(LesserStatement lesserStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(GreaterStatement greaterStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(LEqStatement leqStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(GEqStatement geqStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(NotStatement notStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(NegativeStatement negativeStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(IntStatement intStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(BoolStatement boolStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(TrueStatement trueStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(FalseStatement falseStatement)
        {
            throw new NotImplementedException();
        }

        public IValue Visit(Syntax.Type type)
        {
            throw new NotImplementedException();
        }
    }
}
