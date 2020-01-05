using System;
namespace Syntax
{
    public interface IExpressionVisitorWithArguments<Result, AArgmnt>
    {
        Result Visit(DeclarationSequence declarationSequence,AArgmnt arg);
        Result Visit(SequenceStatement sequenceStatement,AArgmnt arg);
        Result Visit(IdentifierStatement identifierStatement,AArgmnt arg);
        Result Visit(NumStatement numStatement,AArgmnt arg);
        Result Visit(BracketStatement bracketStatement,AArgmnt arg);
        Result Visit(BinOperatorStatement binOperatorStatement,AArgmnt arg);
        Result Visit(ListStatement listStatement,AArgmnt arg);
        Result Visit(FormalList formalList,AArgmnt arg);
        Result Visit(Declaration declaration,AArgmnt arg);
        Result Visit(Argument argument,AArgmnt arg);
        Result Visit(TypeDeclaration typeDeclaration,AArgmnt arg);
        Result Visit(VoidDeclaration voidDeclaration,AArgmnt arg);
        Result Visit(IfStatement ifStatement,AArgmnt arg);
        Result Visit(IfElseStatement ifElseStatement,AArgmnt arg);
        Result Visit(WhileStatement whileStatement,AArgmnt arg);
        Result Visit(BlockStatement blockStatement,AArgmnt arg);
        Result Visit(RegularStatement regularStatement,AArgmnt arg);
        Result Visit(Return rreturn,AArgmnt arg);
        Result Visit(VoidReturn voidReturn,AArgmnt arg);
        Result Visit(AssignStatement assignStatement,AArgmnt arg);
        Result Visit(OrStatement orStatement,AArgmnt arg);
        Result Visit(AndStatement andStatement,AArgmnt arg);
        Result Visit(NotEqStatement notEqStatement,AArgmnt arg);
        Result Visit(EqStatement eqStatement,AArgmnt arg);
        Result Visit(LesserStatement lesserStatement,AArgmnt arg);
        Result Visit(GreaterStatement greaterStatement,AArgmnt arg);
        Result Visit(LEqStatement leqStatement,AArgmnt arg);
        Result Visit(GEqStatement geqStatement,AArgmnt arg);
        Result Visit(NotStatement notStatement,AArgmnt arg);
        Result Visit(NegativeStatement negativeStatement,AArgmnt arg);
        Result Visit(IntStatement intStatement,AArgmnt arg);
        Result Visit(BoolStatement boolStatement,AArgmnt arg);
        Result Visit(TrueStatement trueStatement,AArgmnt arg);
        Result Visit(FalseStatement falseStatement,AArgmnt arg);
        Result Visit(Type type,AArgmnt arg);
    }

    public abstract partial class Statement : Locatable
    {
        public abstract Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg);
    }

    public partial class DeclarationSequence : Declaration
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class SequenceStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class IdentifierStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class NumStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class BracketStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class BinOperatorStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class ListStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class FormalList : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class Declaration : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }



    public partial class Argument : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class TypeDeclaration : Declaration
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }
    public partial class VoidDeclaration : Declaration
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class IfStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class IfElseStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class WhileStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class BlockStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class RegularStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class Return : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class VoidReturn : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class AssignStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }

    }

    public partial class OrStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }

    }
    public partial class AndStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }
    public partial class EqStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }
    public partial class NotEqStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class LesserStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }
    public partial class GreaterStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }
    public partial class LEqStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }
    public partial class GEqStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class NotStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }
    public partial class NegativeStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }
    public partial class IntStatement : Type
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }
    public partial class BoolStatement : Type
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }
    public partial class TrueStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }
    public partial class FalseStatement : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class Type : Statement
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

}