using System;
using System.Collections.Generic;
using Syntax;
namespace Evaluator
{
    public class Void { }

    public class FreeVariablesVisitor : IExpressionVisitorWithArguments<Void, SortedSet<string>>
    {
        public static FreeVariablesVisitor Instance = new FreeVariablesVisitor();

        public Void Visit(SequenceStatement sequenceStatement, SortedSet<string> free)
        {
            sequenceStatement.head.Accept(this, free);
            sequenceStatement.tail.Accept(this, free);

            return null;
        }

        public Void Visit(IdentifierStatement identifierStatement, SortedSet<string> free)
        {
            free.Add(identifierStatement.id);
            return null;
        }

        public Void Visit(NumStatement numberStatement, SortedSet<string> free)
        {
            return null;
        }

        public Void Visit(BinOperatorStatement binaryOperatorStatement, SortedSet<string> free)
        {
            binaryOperatorStatement.left.Accept(this, free);
            binaryOperatorStatement.right.Accept(this, free);

            return null;
        }

        public Void Visit(LetStatement letStatement, SortedSet<string> free)
        {
            letStatement.Expr1.Accept(this, free);

            var bodyFree = new SortedSet<string>();
            letStatement.Expr2.Accept(this, bodyFree);
            bodyFree.Remove(letStatement.Id);

            free.UnionWith(bodyFree);

            return null;
        }

        public Void Visit(IfStatement ifStatement, SortedSet<string> free)
        {
            ifStatement.expr.Accept(this, free);
            ifStatement.stmt.Accept(this, free);

            return null;
        }

        public Void Visit(WhileStatement whileStatement, SortedSet<string> free)
        {
            whileStatement.expr.Accept(this, free);
            whileStatement.stmt.Accept(this, free);

            return null;
        }

        public Void Visit(LambdaStatement lambdaStatement, SortedSet<string> free)
        {
            var bodyFree = new SortedSet<string>();
            lambdaStatement.Body.Accept(this, bodyFree);
            bodyFree.Remove(lambdaStatement.Id);

            free.UnionWith(bodyFree);

            return null;
        }

        public Void Visit(ApplicationStatement applicationStatement, SortedSet<string> free)
        {
            applicationStatement.Function.Accept(this, free);
            applicationStatement.Argument.Accept(this, free);

            return null;
        }

        public Void Visit(TupleStatement tupleStatement, SortedSet<string> free)
        {
            tupleStatement.Expr1.Accept(this, free);
            tupleStatement.Expr2.Accept(this, free);

            return null;
        }

        public Void Visit(ListStatement listStatement, SortedSet<string> free)
        {
            for (var i = 0; i < listStatement.exprs.Count; i++)
            {
                listStatement.exprs[i].Accept(this, free);
            }

            return null;
        }

        public Void Visit(DeclarationSequence declarationSequence, SortedSet<string> free)
        {
            declarationSequence.head.Accept(this,free);
            declarationSequence.tail.Accept(this, free);

            return null;

        }



        public Void Visit(BracketStatement bracketStatement, SortedSet<string> free)
        {
            bracketStatement.stmt.Accept(this, free);

            return null;
        }


        public Void Visit(FormalList formalList, SortedSet<string> free)
        {
            for (var i = 0; i < formalList.list.Count; i++)
            {
                formalList.list[i].Accept(this, free);
            }

            return null;
        }

        public Void Visit(Declaration declaration, SortedSet<string> free)
        {
            return null;
        }

        public Void Visit(Argument freeument, SortedSet<string> free)
        {
            freeument.type.Accept(this, free);
            free.Add(freeument.id);

            return null;
        }

        public Void Visit(TypeDeclaration typeDeclaration, SortedSet<string> free)
        {
            typeDeclaration.type.Accept(this, free);
            var bodyFree = new SortedSet<string>();
            typeDeclaration.flist.Accept(this, bodyFree);
            bodyFree.Remove(typeDeclaration.id);

            free.UnionWith(bodyFree);

            return null;
        }

        public Void Visit(VoidDeclaration voidDeclaration, SortedSet<string> free)
        {
            var bodyFree = new SortedSet<string>();
            voidDeclaration.flist.Accept(this, bodyFree);
            bodyFree.Remove(voidDeclaration.id);

            free.UnionWith(bodyFree);

            return null;
        }

        public Void Visit(IfElseStatement ifElseStatement, SortedSet<string> free)
        {
            ifElseStatement.expr.Accept(this, free);
            ifElseStatement.stmt1.Accept(this, free);
            ifElseStatement.stmt2.Accept(this, free);

            return null;
        }

        public Void Visit(BlockStatement blockStatement, SortedSet<string> free)
        {
            blockStatement.stmt.Accept(this, free);

            return null;
        }

        public Void Visit(RegularStatement regularStatement, SortedSet<string> free)
        {
            regularStatement.expr.Accept(this, free);

            return null;
        }

        public Void Visit(Return rreturn, SortedSet<string> free)
        {
            rreturn.e.Accept(this, free);


            return null;
        }

        public Void Visit(VoidReturn voidReturn, SortedSet<string> free)
        {
            return null;
        }

        public Void Visit(AssignStatement assignStatement, SortedSet<string> free)
        {
            assignStatement.s.Accept(this, free);
            free.Remove(assignStatement.id);

            return null;
        }

        public Void Visit(OrStatement orStatement, SortedSet<string> free)
        {
            orStatement.s1.Accept(this, free);
            orStatement.s2.Accept(this, free);

            return null;

        }

        public Void Visit(AndStatement andStatement, SortedSet<string> free)
        {
            andStatement.s1.Accept(this, free);
            andStatement.s2.Accept(this, free);

            return null;
        }

        public Void Visit(NotEqStatement notEqStatement, SortedSet<string> free)
        {
            notEqStatement.s1.Accept(this, free);
            notEqStatement.s2.Accept(this, free);

            return null;
        }

        public Void Visit(EqStatement eqStatement, SortedSet<string> free)
        {
            eqStatement.s1.Accept(this, free);
            eqStatement.s2.Accept(this, free);

            return null;
        }

        public Void Visit(LesserStatement lesserStatement, SortedSet<string> free)
        {
            lesserStatement.s1.Accept(this, free);
            lesserStatement.s2.Accept(this, free);

            return null;
        }

        public Void Visit(GreaterStatement greaterStatement, SortedSet<string> free)
        {
            greaterStatement.s1.Accept(this, free);
            greaterStatement.s2.Accept(this, free);

            return null;
        }

        public Void Visit(LEqStatement leqStatement, SortedSet<string> free)
        {
            leqStatement.s1.Accept(this, free);
            leqStatement.s2.Accept(this, free);

            return null;
        }

        public Void Visit(GEqStatement geqStatement, SortedSet<string> free)
        {
            geqStatement.s1.Accept(this, free);
            geqStatement.s2.Accept(this, free);

            return null;
        }

        public Void Visit(NotStatement notStatement, SortedSet<string> free)
        {
            notStatement.s1.Accept(this, free);
            
            return null;
        }

        public Void Visit(NegativeStatement negativeStatement, SortedSet<string> free)
        {
            negativeStatement.s1.Accept(this, free);

            return null;
        }

        public Void Visit(IntStatement intStatement, SortedSet<string> free)
        {
            return null;
        }

        public Void Visit(BoolStatement boolStatement, SortedSet<string> free)
        {
            return null;
        }

        public Void Visit(TrueStatement trueStatement, SortedSet<string> free)
        {
            return null;
        }

        public Void Visit(FalseStatement falseStatement, SortedSet<string> free)
        {
            return null;
        }

        public Void Visit(Syntax.Type type, SortedSet<string> free)
        {
            return null;
        }
    }
}