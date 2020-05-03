using System;
using System.Collections.Generic;

using Syntax;

namespace Evaluator
{
    class DesugarState
    {
        Dictionary<string, int> VariableDict = new Dictionary<string, int>();
        Stack<Tuple<string, int>> UnShadowStack = new Stack<Tuple<string, int>>();

        int FindShadow(string variable)
        {
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
            var currentNesting = FindShadow(variable);
            UnShadowStack.Push(new Tuple<string, int>(variable, currentNesting + 1));
        }

        public void PopPossibleShadow()
        {
            UnShadowStack.Pop();
        }

        public string UnShadow(string variable)
        {
            var currentNesting = FindShadow(variable);

            return currentNesting > 0 ? $"{variable}_{currentNesting}" : variable;
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
            sequenceStatement.head = sequenceStatement.head.Accept(this);
            sequenceStatement.tail = sequenceStatement.tail.Accept(this);

            return sequenceStatement;
        }

        public Statement Visit(IdentifierStatement identifierStatement)
        {
            identifierStatement.id = State.UnShadow(identifierStatement.id);
            return identifierStatement;
        }

        public Statement Visit(NumStatement numberStatement)
        {
            return numberStatement;
        }

        public Statement Visit(BlockStatement blockStatement)
        {
            blockStatement.stmt.Accept(this);
            return null;
        }

        public Statement Visit(BinOperatorStatement binaryOperatorStatement)
        {
            binaryOperatorStatement.left = binaryOperatorStatement.left.Accept(this);
            binaryOperatorStatement.right = binaryOperatorStatement.right.Accept(this);

            return binaryOperatorStatement;
        }

        public Statement Visit(IfStatement ifStatement)
        {
            ifStatement.expr = ifStatement.expr.Accept(this);
            ifStatement.stmt = ifStatement.stmt.Accept(this);

            return ifStatement;
        }

        public Statement Visit(IfElseStatement ifElseStatement)
        {
            ifElseStatement.expr = ifElseStatement.expr.Accept(this);
            ifElseStatement.stmt1 = ifElseStatement.stmt1.Accept(this);
            ifElseStatement.stmt2 = ifElseStatement.stmt2.Accept(this);

            return ifElseStatement;
        }

        public Statement Visit(WhileStatement whileStatement)
        {
            whileStatement.expr = whileStatement.expr.Accept(this);
            whileStatement.stmt = whileStatement.stmt.Accept(this);

            return whileStatement;
        }

        public Statement Visit(ListStatement listStatement)
        {
            for (var i = 0; i < listStatement.exprs.Count; i++)
            {
                listStatement.exprs[i] = listStatement.exprs[i].Accept(this);
            }

            return listStatement;
        }

        public Statement Visit(DeclarationSequence declarationSequence)
        {
            declarationSequence.head = declarationSequence.head.Accept(this);
            declarationSequence.tail = declarationSequence.tail.Accept(this);

            return declarationSequence;
        }


        public Statement Visit(FormalList formalList)
        {
            for (var i = 0; i < formalList.list.Count; i++)
            {
                formalList.list[i] = (Argument) formalList.list[i].Accept(this);
            }

            return formalList;
        }

        public Statement Visit(Declaration declaration)
        {
            return declaration;
        }

        public Statement Visit(Argument argument)
        {
            argument.type = argument.type.Accept(this);
            argument.id=State.UnShadow(argument.id);
            return argument;
        }

        public Statement Visit(TypeDeclaration typeDeclaration)
        {
            typeDeclaration.type = typeDeclaration.type.Accept(this);
            typeDeclaration.flist = typeDeclaration.flist.Accept(this);
            typeDeclaration.stmt = typeDeclaration.stmt.Accept(this);

            return typeDeclaration;
        }

        public Statement Visit(VoidDeclaration voidDeclaration)
        {
            voidDeclaration.flist = voidDeclaration.flist.Accept(this);
            voidDeclaration.stmt = voidDeclaration.stmt.Accept(this);

            return voidDeclaration;
        }


        public Statement Visit(RegularStatement regularStatement)
        {
            regularStatement.expr = regularStatement.expr.Accept(this);

            return regularStatement;
        }

        public Statement Visit(Return rreturn)
        {
            rreturn.e = rreturn.e.Accept(this);

            return rreturn;
        }

        public Statement Visit(VoidReturn voidReturn)
        {
            return voidReturn;
        }

        public Statement Visit(AssignStatement assignStatement)
        {
            assignStatement.id= State.UnShadow(assignStatement.id);
            assignStatement.s = assignStatement.s.Accept(this);

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
            return boolStatement;
        }  

        public Statement Visit(Syntax.Type type)
        {
            return type;
        }
    }
}
