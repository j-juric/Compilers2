﻿using System;
namespace Syntax
{
    public interface IExpressionVisitorWithArguments<Result, AArgmnt>
    {
        Result Visit(DeclarationSequence declarationSequence,AArgmnt arg);
        Result Visit(SequenceStatement sequenceStatement,AArgmnt arg);
        Result Visit(IdentifierStatement identifierStatement,AArgmnt arg);
        Result Visit(NumStatement numStatement,AArgmnt arg);
        Result Visit(BinOperatorStatement binOperatorStatement,AArgmnt arg);
        Result Visit(ListStatement listStatement,AArgmnt arg);
        Result Visit(FormalList formalList,AArgmnt arg);
        Result Visit(Declaration declaration,AArgmnt arg);
        Result Visit(BlockStatement blockStatement,AArgmnt arg);
        Result Visit(Argument argument,AArgmnt arg);
        Result Visit(TypeDeclaration typeDeclaration,AArgmnt arg);
        Result Visit(VoidDeclaration voidDeclaration,AArgmnt arg);
        Result Visit(IfStatement ifStatement,AArgmnt arg);
        Result Visit(IfElseStatement ifElseStatement,AArgmnt arg);
        Result Visit(WhileStatement whileStatement,AArgmnt arg);
        Result Visit(RegularStatement regularStatement,AArgmnt arg);
        Result Visit(Return rreturn,AArgmnt arg);
        Result Visit(VoidReturn voidReturn,AArgmnt arg);
        Result Visit(AssignStatement assignStatement,AArgmnt arg);
        Result Visit(NotStatement notStatement,AArgmnt arg);
        Result Visit(NegativeStatement negativeStatement,AArgmnt arg);
        Result Visit(IntType intType,AArgmnt arg);
        Result Visit(BoolType boolType,AArgmnt arg);
        Result Visit(BoolStatement boolStatement,AArgmnt arg);
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

    public partial class BlockStatement : Statement
    {
        override public Result Accept<Result, AArgmnt>(IExpressionVisitorWithArguments<Result, AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this, arg);
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
    public partial class IntType : Type
    {
        override public Result Accept<Result,AArgmnt>(IExpressionVisitorWithArguments<Result,AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this,arg);
        }
    }

    public partial class BoolType : Type
    {
        override public Result Accept<Result, AArgmnt>(IExpressionVisitorWithArguments<Result, AArgmnt> visitor, AArgmnt arg)
        {
            return visitor.Visit(this, arg);
        }
    }
    public partial class BoolStatement : Statement
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