using System;
using System.Text;
using System.Collections.Generic;
using QUT.Gppg;

namespace Syntax
{
    public interface IExpressionVisitor<Result>
    {
        Result Visit(DeclarationSequence declarationSequence);
        Result Visit(SequenceStatement sequenceStatement);
        Result Visit(IdentifierStatement identifierStatement);
        Result Visit(NumStatement numStatement);
        Result Visit(BlockStatement blockStatement);
        Result Visit(BinOperatorStatement binOperatorStatement);
        Result Visit(ListStatement listStatement);
        Result Visit(FormalList formalList);
        Result Visit(Declaration declaration);
        Result Visit(Argument argument);
        Result Visit(TypeDeclaration typeDeclaration);
        Result Visit(VoidDeclaration voidDeclaration);
        Result Visit(IfStatement ifStatement);
        Result Visit(IfElseStatement ifElseStatement);
        Result Visit(WhileStatement whileStatement);
        Result Visit(RegularStatement regularStatement);
        Result Visit(Return rreturn);
        Result Visit(VoidReturn voidReturn);
        Result Visit(AssignStatement assignStatement);
        Result Visit(NotStatement notStatement);
        Result Visit(NegativeStatement negativeStatement);
        Result Visit(IntType intType);
        Result Visit(BoolType boolType);
        Result Visit(BoolStatement boolStatement);

        Result Visit(Type type);
    }

    public abstract partial class Statement : Locatable
    {
        public abstract Result Accept<Result>(IExpressionVisitor<Result> visitor);
    }

    public partial class DeclarationSequence : Declaration
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class SequenceStatement : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class BlockStatement : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class IdentifierStatement : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class NumStatement : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }


    public partial class BinOperatorStatement : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class ListStatement : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class FormalList : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class Declaration : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }



    public partial class Argument : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class TypeDeclaration : Declaration
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            
            return visitor.Visit(this);
        }
    }
    public partial class VoidDeclaration : Declaration
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class IfStatement : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class IfElseStatement : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class WhileStatement : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }


    public partial class RegularStatement : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class Return : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class VoidReturn : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class AssignStatement : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }

    }


    public partial class NotStatement : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }
    public partial class NegativeStatement : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }
    public partial class IntType : Type
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }
    public partial class BoolType : Type
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class BoolStatement : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public partial class Type : Statement
    {
        override public Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            return visitor.Visit(this);
        }
    }
}