using System;
using System.Collections.Generic;

using Syntax;

namespace Evaluator
{
    class DesugarState
    {
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

        public Statement Visit(BracketStatement bracketStatement)
        {
            bracketStatement.stmt = bracketStatement.stmt.Accept(this);

            return bracketStatement;
        }

        public Statement Visit(FormalList formalList)
        {
            for (var i = 0; i < formalList.list.Count; i++)
            {
                formalList.list[i] = formalList.list[i].Accept(this);
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

        public Statement Visit(BlockStatement blockStatement)
        {
            blockStatement.stmt = blockStatement.stmt.Accept(this);

            return blockStatement;
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

        public Statement Visit(OrStatement orStatement)
        {
            orStatement.s1 = orStatement.s1.Accept(this);
            orStatement.s2 = orStatement.s2.Accept(this);

            return orStatement;
        }

        public Statement Visit(AndStatement andStatement)
        {
            andStatement.s1 = andStatement.s1.Accept(this);
            andStatement.s2 = andStatement.s2.Accept(this);

            return andStatement;
        }

        public Statement Visit(NotEqStatement notEqStatement)
        {
            notEqStatement.s1 = notEqStatement.s1.Accept(this);
            notEqStatement.s2 = notEqStatement.s2.Accept(this);

            return notEqStatement;
        }

        public Statement Visit(EqStatement eqStatement)
        {
            eqStatement.s1 = eqStatement.s1.Accept(this);
            eqStatement.s2 = eqStatement.s2.Accept(this);

            return eqStatement;
        }

        public Statement Visit(LesserStatement lesserStatement)
        {
            lesserStatement.s1 = lesserStatement.s1.Accept(this);
            lesserStatement.s2 = lesserStatement.s2.Accept(this);

            return lesserStatement;
        }

        public Statement Visit(GreaterStatement greaterStatement)
        {
            greaterStatement.s1 = greaterStatement.s1.Accept(this);
            greaterStatement.s2 = greaterStatement.s2.Accept(this);

            return greaterStatement;
        }

        public Statement Visit(LEqStatement leqStatement)
        {
            leqStatement.s1 = leqStatement.s1.Accept(this);
            leqStatement.s2 = leqStatement.s2.Accept(this);

            return leqStatement;
        }

        public Statement Visit(GEqStatement geqStatement)
        {
            geqStatement.s1 = geqStatement.s1.Accept(this);
            geqStatement.s2 = geqStatement.s2.Accept(this);

            return geqStatement;
        }

        public Statement Visit(NotStatement notStatement)
        {
            notStatement.s1 = notStatement.s1.Accept(this);

            return notStatement;
        }

        public Statement Visit(NegativeStatement negativeStatement)
        {
            negativeStatement.s1 = negativeStatement.s1.Accept(this);

            return negativeStatement;
        }

        public Statement Visit(IntStatement intStatement) //mozda je bag
        {
            return intStatement;
        }

        public Statement Visit(BoolStatement boolStatement)
        {
            return boolStatement;
        }

        public Statement Visit(TrueStatement trueStatement)
        {
            return trueStatement;
        }

        public Statement Visit(FalseStatement falseStatement)
        {
            return falseStatement;
        }

        public Statement Visit(Syntax.Type type)
        {
            return type;
        }
    }
}
