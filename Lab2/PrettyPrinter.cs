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

    public partial class BoolStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append(this.value.ToString().ToLower());
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
                        {Type.SUB, "-" },
                        {Type.AND, "&&" },
                        {Type.OR, "||" },
                        {Type.GR, ">" },
                        {Type.LE, "<" },
                        {Type.LEQ, "<=" },
                        {Type.GEQ, ">=" },
                        {Type.EQ, "==" },
                        {Type.NEQ, "!=" }
                    };

        static Dictionary<Type, int> Precedences =
            new Dictionary<Type, int>()
            {
                {Type.ADD, 6},
                {Type.SUB, 6},
                {Type.DIV, 7},
                {Type.MUL, 7},
                {Type.AND, 3 },
                {Type.OR, 2 },
                {Type.GR, 5 },
                {Type.LE, 5 },
                {Type.LEQ, 5 },
                {Type.GEQ, 5 },
                {Type.EQ, 4 },
                {Type.NEQ, 4 }
            };

        enum Associativity { Left, Right, Both }

        static Dictionary<Type, Associativity> Associativities =
            new Dictionary<Type, Associativity>()
            {
                {Type.ADD, Associativity.Left},
                {Type.SUB, Associativity.Left},
                {Type.DIV, Associativity.Left},
                {Type.MUL, Associativity.Left},
                {Type.AND, Associativity.Left },
                {Type.OR, Associativity.Left },
                {Type.GR, Associativity.Left },
                {Type.LE, Associativity.Left },
                {Type.LEQ, Associativity.Left },
                {Type.GEQ, Associativity.Left },
                {Type.EQ, Associativity.Left },
                {Type.NEQ, Associativity.Left }
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
            var needsParenthesis = outerPrecedence > 1 ||
                    opposite && (outerPrecedence == 1);

            if (needsParenthesis)
            {
                builder.Append("(");
            }
            builder.Append(id);
            builder.Append(" = ");
            s.Pretty(builder, 1, false);

            if (needsParenthesis)
            {
                builder.Append(")");
            }
            //if (outerPrecedence == 0)
            //{
            //    builder.Append(" ;");
            //}
        }
    }

    

    public partial class NotStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            var needsParenthesis = outerPrecedence > 9;

            builder.Append("!");
            if (needsParenthesis)
            {
                builder.Append("(");
            }
            s1.Pretty(builder, 9, false);
            if (needsParenthesis)
            {
                builder.Append(")");
            }
        }
    }

    public partial class NegativeStatement : Statement
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            var needsParenthesis = outerPrecedence > 9;

            builder.Append("-");
            if (needsParenthesis)
            {
                builder.Append("(");
            }
            s1.Pretty(builder, 9, false);
            if (needsParenthesis)
            {
                builder.Append(")");
            }
        }
    }
    public partial class IntType : Type
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append("int");
        }
    }
    public partial class BoolType : Type
    {
        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            builder.Append("bool");
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
