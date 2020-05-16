using System;
using System.Collections.Generic;

using Syntax;

namespace Evaluator
{
    class DesugarState
    {
        Dictionary<string, int> VarCount = new Dictionary<string, int>(); //Dictionary that counts the variables with the same name
        Stack<Tuple<string, int>> UnShadowStack = new Stack<Tuple<string, int>>(); //Variable stack
        Stack<int> VarInScope = new Stack<int>(); //Stack that counts variables in each scope

        int FindShadow(string variable)
        {
            //Console.WriteLine("Find Shadow!");
            foreach (var mapping in UnShadowStack)
            {
                if (mapping.Item1 == variable)
                {
                    return mapping.Item2;
                }
            }

            return 0;
        }

        public void PushPossibleShadow(string variable)
        {
            //Console.WriteLine("PUSH_POS_SHADOW!");
            //Insert var in dictionary
            if (VarCount.ContainsKey(variable))
            {
                VarCount[variable]++;
                var currentNesting = VarCount[variable];
                 UnShadowStack.Push(new Tuple<string, int>(variable, currentNesting));
            }
            else
            {
                VarCount.Add(variable, 1);
                UnShadowStack.Push(new Tuple<string, int>(variable, 1));
            }


            // Increment current number of variables in the scope
            var numOfVarsInScope = VarInScope.Pop();
            VarInScope.Push(numOfVarsInScope + 1);

        }

        public void ClearState()
        {
            //Console.WriteLine("Clear STATE!");
            UnShadowStack.Clear();
            VarCount.Clear();
        }

        public string UnShadow(string variable)
        {
            //Console.WriteLine("Unshadow!");
            var currentNesting = FindShadow(variable);

            return currentNesting > 0 ? $"{variable}_{currentNesting}" : variable;
        }

        public void ScopeStart()
        {
            //Console.WriteLine("Scope start!");
            this.VarInScope.Push(0);
        }

        public void ScopeEnd()
        {
            //Console.WriteLine("Scope end!");
            var numOfVar = this.VarInScope.Pop();
            while (numOfVar > 0)
            {
                this.UnShadowStack.Pop();
                numOfVar--;
            }
        }

        public void Print()
        {
            Console.WriteLine("===Dictionary<string, int> VarCount===");
            foreach(var variable in this.VarCount)
            {
                Console.WriteLine($"{variable.Key} : {variable.Value}");
            }
            Console.WriteLine("===Stack<Tuple<string, int>> UnShadowStack===");
            foreach (var variable in this.UnShadowStack)
            {
                Console.WriteLine($"{variable.Item1} : {variable.Item2}");
            }            
            Console.WriteLine("=== Stack<int> VarInScope===");
            foreach (var variable in this.VarInScope)
            {
                Console.Write($"{variable},");
            }
            Console.WriteLine();
        }

    }

    // ---
    // Note that the DesugarVisitor does a destructive update, despite returning
    // an expression.

    public class DesugarVisitor : IExpressionVisitor<Statement>
    {

        DesugarState State = new DesugarState();

        public Statement Visit(SequenceStatement sequenceStatement)
        {
            //State.ScopeStart();
            //Console.WriteLine("SEQUENCE STATEMENT TAIL");

            if(sequenceStatement.head != null)
                sequenceStatement.head.Accept(this);
            sequenceStatement.tail.Accept(this);
            //State.ScopeEnd();
            return null;
            
        }

        public Statement Visit(IdentifierStatement identifierStatement)
        {
            if (identifierStatement.list == null)
            {
                identifierStatement.id = State.UnShadow(identifierStatement.id);
                //Console.WriteLine($"**********IDENTIFIER STATMENT:{identifierStatement.id}**********");
            }
            else
            {
                foreach (var e in ((ListStatement)identifierStatement.list).exprs)
                {
                    e.Accept(this);
                }
            }
            return null;
        }

        public Statement Visit(NumStatement numberStatement)
        {
            return null;
        }

        public Statement Visit(BlockStatement blockStatement)
        {
            //Console.WriteLine("BLOCK STATEMENT!");
            State.ScopeStart();
            blockStatement.stmt.Accept(this);
            //State.Print();
            State.ScopeEnd();
            return null;
            
        }

        public Statement Visit(BinOperatorStatement binaryOperatorStatement)
        {
            binaryOperatorStatement.left.Accept(this);
           binaryOperatorStatement.right.Accept(this);

            return null;
        }

        public Statement Visit(IfStatement ifStatement)
        {
            ifStatement.expr.Accept(this);
            ifStatement.stmt.Accept(this);

            return null;
        }

        public Statement Visit(IfElseStatement ifElseStatement)
        {
            ifElseStatement.expr.Accept(this);
             ifElseStatement.stmt1.Accept(this);
             ifElseStatement.stmt2.Accept(this);

            return null;
        }

        public Statement Visit(WhileStatement whileStatement)
        {
            whileStatement.expr.Accept(this);
            whileStatement.stmt.Accept(this);

            return null;
        }

        public Statement Visit(ListStatement listStatement)
        {

            return null;
        }

        public Statement Visit(DeclarationSequence declarationSequence)
        {
            var declaration = declarationSequence;
            while (declaration != null)
            {
                var typeName = declaration.tail.GetType().Name;
                State.ClearState();
                if (typeName == "VoidDeclaration")
                   ((VoidDeclaration)declaration.tail).Accept(this);
                else
                    ((TypeDeclaration)declaration.tail).Accept(this);
                declaration = (DeclarationSequence)declaration.head;

            }

            return null;
        }


        public Statement Visit(FormalList formalList)
        {
            //for (var i = 0; i < formalList.list.Count; i++)
            //{
            //    formalList.list[i] = (Argument) formalList.list[i].Accept(this);
            //}

            return null;
        }

        public Statement Visit(Declaration declaration)
        {
            return null;
        }

        public Statement Visit(Argument argument)
        {
            State.PushPossibleShadow(argument.id);
            argument.id=State.UnShadow(argument.id);
            //Console.WriteLine($"**********ARGUMENT STATMENT:{argument.id}**********");
            return null;
        }

        public Statement Visit(TypeDeclaration typeDeclaration)
        {
            State.ScopeStart();
            if (typeDeclaration.stmt!=null)
                typeDeclaration.stmt.Accept(this);
            State.ScopeEnd();
            State.ClearState();
            
            return null;
        }

        public Statement Visit(VoidDeclaration voidDeclaration)
        {
            //Console.WriteLine("VOID DECL");
            State.ScopeStart();
            voidDeclaration.stmt.Accept(this);
            //State.Print();
            State.ScopeEnd();
            State.ClearState();
            return null;
        }


        public Statement Visit(RegularStatement regularStatement)
        {
             regularStatement.expr.Accept(this);

            return null;
        }

        public Statement Visit(Return rreturn)
        {
           rreturn.e.Accept(this);

            return null;
        }

        public Statement Visit(VoidReturn voidReturn)
        {
            return null;
        }

        public Statement Visit(AssignStatement assignStatement)
        {
            assignStatement.id = State.UnShadow(assignStatement.id);
            //Console.WriteLine($"**********Assign STATMENT:{assignStatement.id} **********");
            assignStatement.s.Accept(this);

            return assignStatement;
        }

      

        public Statement Visit(NotStatement notStatement)
        {
            notStatement.s1.Accept(this);

            return null;
        }

        public Statement Visit(NegativeStatement negativeStatement)
        {
            negativeStatement.s1.Accept(this);

            return null;
        }

        public Statement Visit(IntType intType) //mozda je bag
        {
            return null;
        }        
        public Statement Visit(BoolType boolType) //mozda je bag
        {
            return null;
        }

        public Statement Visit(BoolStatement boolStatement)
        {
            return null;
        }  

        public Statement Visit(Syntax.Type type)
        {
            return null;
        }
    }
}
