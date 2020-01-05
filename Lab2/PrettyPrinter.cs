using System;
using System.Text;
using System.Collections.Generic;
using Lab2;

namespace Syntax
{

    public partial class DeclarationSequence : Declaration
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            if (head != null)
            {
                head.Pretty(builder, 0, false);
                builder.NewLine();
            }
            if (tail != null)
                tail.Pretty(builder, 0, false);
        }
    }
    public partial class SequenceStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            if (head != null)
            {
                head.Pretty(builder, 0, false);
                //if(!head.complexStatement)
                //    builder.Append(";");
                builder.NewLine();
            }
            if (tail != null)
            {
                tail.Pretty(builder, 0, false);
                if (!tail.complexStatement)
                    builder.Append(";");
            }
                
        }
    }

    public partial class IdentifierStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append(id);
            if (list != null)
            {
                builder.Append("(");
                if (list != null)
                    list.Pretty(builder, 0, false);
                builder.Append(")");

            }

        }
    }

    public partial class NumStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append(num.ToString());
        }
    }

    public partial class BracketStatement : Statement
    {
       
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append("(");
            if (stmt != null)
                stmt.Pretty(builder, 0, false);
            builder.Append(")");
        }
    }

    public partial class BinOperatorStatement : Statement
    {
        static Dictionary<Type, string> Operators =
                    new Dictionary<Type, string>()
         {
            {Type.ADD, "+" },
            {Type.DIV, "/" },
            {Type.MUL, "*" },
            {Type.SUB, "-" }
          };

        static Dictionary<Type, int> Precedences =
            new Dictionary<Type, int>()
            {
                {Type.ADD, 7},
                {Type.SUB, 7},
                {Type.DIV, 8},
                {Type.MUL, 8}
            };

        enum Associativity { Left, Right, Both }

        static Dictionary<Type, Associativity> Associativities =
            new Dictionary<Type, Associativity>()
            {
                {Type.ADD, Associativity.Both},
                {Type.SUB, Associativity.Left},
                {Type.DIV, Associativity.Left},
                {Type.MUL, Associativity.Both}
            };
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            var needsParenthesis = outerPrecedence > Precedences[type] ||
                    opposite && (outerPrecedence == Precedences[type]);

            if (needsParenthesis)
            {
                builder.Append("(");
            }

            left.Pretty(builder, Precedences[type], Associativities[type] == Associativity.Right);
            builder.Append(Operators[type]);
            right.Pretty(builder, Precedences[type], Associativities[type] == Associativity.Left);

            if (needsParenthesis)
            {
                builder.Append(")");
            }
        }
    }

    public partial class ListStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            var first = true;

            foreach (var expr in exprs)
            {
                if (!first) builder.Append(", ");
                else first = false;

                expr.Pretty(builder, 0, false);
            }
        }
    }

    public partial class FormalList : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            var first = true;

            foreach (var arg in list)
            {
                if (!first)
                {
                    builder.Append(", ");
                }
                first = false;
                arg.Pretty(builder, 0, false);
            }
        }
    }

    public partial class Declaration : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
        }
    }


    public partial class Argument : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            this.type.Pretty(builder, 0, false);
            builder.Append(" ");
            builder.Append(this.id);
            //builder.Append(this.type.ToString()+" "+this.id);
        }
    }

    public partial class TypeDeclaration : Declaration
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            type.Pretty(builder, 0, false);
            builder.Append(" ");
            builder.Append(id);
            builder.Append("(");
            flist.Pretty(builder, 0, false);
            builder.Append(") ");
            builder.NewLine();
            builder.Append("{");
            builder.Indent();
            builder.NewLine();
            if (stmt != null) 
                stmt.Pretty(builder, 0, false);
            builder.Unindent();
            builder.NewLine();
            builder.Append("}");
        }
    }
    public partial class VoidDeclaration : Declaration
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append("void ");
            builder.Append(id);
            builder.Append("(");
            flist.Pretty(builder, 0, false);
            builder.Append(") ");
            builder.NewLine();
            builder.Append("{");
            builder.Indent();
            builder.NewLine();
            if (stmt!=null) 
                stmt.Pretty(builder, 0, false);
            builder.Unindent();
            builder.NewLine();
            builder.Append("}");
            builder.NewLine();
        }
    }

    public partial class IfStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append("if (");
            expr.Pretty(builder, 0, false);
            builder.Append(")");           
            builder.NewLine();
            //builder.Indent();
            if (stmt != null) 
                stmt.Pretty(builder, 0, false);
            //builder.Unindent();
        }
    }

    public partial class IfElseStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append("if (");
            expr.Pretty(builder, 0, false);
            builder.Append(")");
            //builder.Indent();
            builder.NewLine();
            if (stmt1 != null) 
                stmt1.Pretty(builder, 0, false);
            //builder.Unindent();
            builder.NewLine();
            builder.Append("else");
            //builder.Indent();
            builder.NewLine();
            if (stmt2 != null) 
                stmt2.Pretty(builder, 0, false);
            //builder.Unindent();
        }
    }

    public partial class WhileStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append("while (");
            expr.Pretty(builder, 0, false);
            builder.Append(")");
            //builder.Indent();
            builder.NewLine();
            if (stmt != null) 
                stmt.Pretty(builder, 0, false);
            //builder.Unindent();    
        }
    }

    public partial class BlockStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append("{");
            if (stmt != null)
            {
                builder.Indent();
                builder.NewLine();
                if (stmt != null)
                    stmt.Pretty(builder, 0, false);
                builder.Unindent();
                builder.NewLine();
            }
            else
            {
                builder.NewLine();
            }
            builder.Append("}");
        }
    }

    public partial class RegularStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            expr.Pretty(builder, 0, false);
            //builder.Append(" ;");
            //builder.NewLine();
        }
    }
    public partial class Return : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append("return ");
            e.Pretty(builder, 0, false);
            //builder.Append(" ;");
            //builder.NewLine();
        }
    }

    public partial class VoidReturn : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append("return "); //;
            
        }
    }

    public partial class AssignStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append(id);
            builder.Append(" = ");
            s.Pretty(builder, 1, false);
            //if (outerPrecedence == 0)
            //{
            //    builder.Append(" ;");
            //}
        }
    }

    public partial class OrStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            var needsParenthesis = outerPrecedence > 2;

            if (needsParenthesis)
            {
                builder.Append("(");
            }

            s1.Pretty(builder, 2, false);
            builder.Append("||");
            s2.Pretty(builder, 2, false);

            if (needsParenthesis)
            {
                builder.Append(")");
            }
        }
    }
    public partial class AndStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            var needsParenthesis = outerPrecedence > 3;

            if (needsParenthesis)
            {
                builder.Append("(");
            }

            s1.Pretty(builder, 3, false);
            builder.Append("&&");
            s2.Pretty(builder, 3, false);

            if (needsParenthesis)
            {
                builder.Append(")");
            }
        }
    }
    public partial class EqStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            var needsParenthesis = outerPrecedence > 4;

            if (needsParenthesis)
            {
                builder.Append("(");
            }

            s1.Pretty(builder, 4, false);
            builder.Append("==");
            s2.Pretty(builder, 4, false);

            if (needsParenthesis)
            {
                builder.Append(")");
            }
        }
    }
    public partial class NotEqStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            var needsParenthesis = outerPrecedence > 4;

            if (needsParenthesis)
            {
                builder.Append("(");
            }

            s1.Pretty(builder, 4, false);
            builder.Append("!=");
            s2.Pretty(builder, 4, false);

            if (needsParenthesis)
            {
                builder.Append(")");
            }
        }
    }
    public partial class LesserStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            var needsParenthesis = outerPrecedence > 5;

            if (needsParenthesis)
            {
                builder.Append("(");
            }

            s1.Pretty(builder, 5, false);
            builder.Append("<");
            s2.Pretty(builder, 5, false);

            if (needsParenthesis)
            {
                builder.Append(")");
            }
        }
    }
    public partial class GreaterStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            var needsParenthesis = outerPrecedence > 5;

            if (needsParenthesis)
            {
                builder.Append("(");
            }

            s1.Pretty(builder, 5, false);
            builder.Append(">");
            s2.Pretty(builder, 5, false);

            if (needsParenthesis)
            {
                builder.Append(")");
            }
        }
    }
    public partial class LEqStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            var needsParenthesis = outerPrecedence > 5;

            if (needsParenthesis)
            {
                builder.Append("(");
            }

            s1.Pretty(builder, 5, false);
            builder.Append("<=");
            s2.Pretty(builder, 5, false);

            if (needsParenthesis)
            {
                builder.Append(")");
            }
        }
    }
    public partial class GEqStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            var needsParenthesis = outerPrecedence > 5;

            if (needsParenthesis)
            {
                builder.Append("(");
            }

            s1.Pretty(builder, 5, false);
            builder.Append(">=");
            s2.Pretty(builder, 5, false);

            if (needsParenthesis)
            {
                builder.Append(")");
            }
        }
    }

    public partial class NotStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            //var needsParenthesis = outerPrecedence > 5;

            builder.Append("!");
            //if (needsParenthesis)
            //{
            //    builder.Append("(");
            //}
            s1.Pretty(builder, 5, false);
            //if (needsParenthesis)
            //{
            //    builder.Append(")");
            //}
        }
    }

    public partial class NegativeStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            //var needsParenthesis = outerPrecedence > 5;

            builder.Append("-");
            //if (needsParenthesis)
            //{
            //    builder.Append("(");
            //}
            s1.Pretty(builder, 5, false);
            //if (needsParenthesis)
            //{
            //    builder.Append(")");
            //}
        }
    }
    public partial class IntStatement : Type
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append("int");
        }
    }
    public partial class BoolStatement : Type
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append("bool");
        }
    }
    public partial class TrueStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append("true");
        }
    }
    public partial class FalseStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append("false");
        }
    }

    public partial class Type : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append(ToString());
        }
    }

}
