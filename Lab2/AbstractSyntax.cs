﻿using System;
using System.Text;
using System.Collections.Generic;
using Lab2;

namespace Syntax
{
    public class Locatable
    {
        public int line, column;

        public void SetLocation(QUT.Gppg.LexLocation loc)
        {
            if (loc != null)
            {
                line = loc.StartLine;
                column = loc.StartColumn;
            }

        }
    }

    public abstract partial class Statement : Locatable
    {
        public bool complexStatement = false;
        public abstract void Pretty(Lab2.PrettyBuilder builder, int outerPrecedence, bool opposite);

        public string Pretty()
        {
            var builder = new Lab2.PrettyBuilder();
            Pretty(builder, 0, false);
            return builder.ToString();
        }
    }
    //
    public partial class DeclarationSequence : Declaration
    {
        public Statement head;
        public Statement tail;

        public DeclarationSequence(Declaration head, Declaration tail)
        {
            this.head = head;
            this.tail = tail;
        }
    }

    public partial class SequenceStatement : Statement
    {
        public Statement head;
        public Statement tail;

        public SequenceStatement(Statement head, Statement tail)
        {
            this.head = head;
            this.tail = tail;
        }
    }

    public partial class IdentifierStatement : Statement
    {
        public string id;
        public Statement list;

        public IdentifierStatement(string id, Statement list)
        {
            this.id = id;
            this.list = list;
            complexStatement = false;
        }

    }


    public partial class BinOperatorStatement : Statement
    {
        public Type type;
        public Statement left;
        public Statement right;

        public enum Type { ADD, SUB, MUL, DIV, OR, AND, EQ, NEQ, LE, GR, LEQ, GEQ }

        public BinOperatorStatement(Type type, Statement left, Statement right)
        {
            this.type = type;
            this.left = left;
            this.right = right;
            complexStatement = false;
        }

  
    }

    public partial class ListStatement : Statement
    {
        public List<Statement> exprs;

        public ListStatement(List<Statement> exprs)
        {
            this.exprs = exprs;
            complexStatement = false;
        }
    }

    public partial class FormalList : Statement
    {
        public List<Argument> list;
        public FormalList(List<Argument> list)
        {
            this.list = list;        
        }
    }
    
    public partial class Declaration : Statement
    {

        public Declaration() { complexStatement = false; }


    }

    public partial class Argument : Statement 
    {
        public Statement type;
        public string id;
        public Argument(Type type, string id)
        {
            this.type = type;
            this.id = id;
        }

    }

    public partial class TypeDeclaration : Declaration
    {
        public Statement type;
        public string id;
        public Statement flist;
        public Statement stmt;

        public TypeDeclaration(Type type, string id, FormalList flist, Statement stmt)
        {
            this.type = type;
            this.id = id;
            this.flist = flist;
            this.stmt = stmt;
        }

    }

    public partial class VoidDeclaration : Declaration
    {
        public string id;
        public Statement flist;
        public Statement stmt;

        public VoidDeclaration(string id, FormalList flist, Statement stmt)
        {

            this.id = id;
            this.flist = flist;
            this.stmt = stmt;
        }


    }

    public partial class IfStatement : Statement
    {
        public Statement expr;
        public Statement stmt;

        public IfStatement(Statement e, Statement s)
        {
            expr = e;
            stmt = s;
            complexStatement = true;
        }


    }

    public partial class IfElseStatement : Statement
    {
        public Statement expr;
        public Statement stmt1;
        public Statement stmt2;
        public IfElseStatement(Statement e, Statement s1, Statement s2)
        {
            expr = e;
            stmt1 = s1;
            stmt2 = s2;
            complexStatement = true;
        }

    }

    public partial class WhileStatement : Statement
    {
        public Statement expr;
        public Statement stmt;

        public WhileStatement(Statement e, Statement s)
        {
            expr = e;
            stmt = s;
            complexStatement = true;
        }

    }

    public partial class BlockStatement : Statement
    {
        public Statement stmt;

        public BlockStatement(Statement s)
        {
            stmt = s;
            complexStatement = true;
        }


    }

    public partial class IntStatement:Statement
    {
        public IntStatement() { }

        public override Result Accept<Result, AArgmnt>(IExpressionVisitorWithArguments<Result, AArgmnt> visitor, AArgmnt arg)
        {
            throw new NotImplementedException();
        }

        public override Result Accept<Result>(IExpressionVisitor<Result> visitor)
        {
            throw new NotImplementedException();
        }

        public override void Pretty(PrettyBuilder builder, int outerPrecedence, bool opposite)
        {
            throw new NotImplementedException();
        }
    }

    public partial class RegularStatement : Statement
    {
        public Statement expr;

        public RegularStatement(Statement expr)
        {
            this.expr = expr;
        }

    }

    public partial class Return : Statement
    {
        public Statement e;
        public Return(Statement e) 
        {
            this.e = e;
        }


    }

    public partial class VoidReturn : Statement
    {
       
        public VoidReturn() {}

    }

    public partial class AssignStatement : Statement
    {
        public string id;
        public Statement s;

        public AssignStatement(string id, Statement s)
        {
            this.id = id;
            this.s = s;
        }

    }

    public partial class NotStatement : Statement
    {
        public Statement s1;
        public NotStatement(Statement s1)
        {
            this.s1 = s1;
        }

    }

    public partial class NegativeStatement : Statement
    {
        public Statement s1;
        public NegativeStatement(Statement s1)
        {
            this.s1 = s1;
        }

    }

    public partial class IntType : Type
    {
        public IntType()
        {
            this.name = "int";
        }

        public override string ToString()
        {
            return this.name;
        }
    }

    public partial class BoolType : Type
    {
        public BoolType()
        {
            this.name = "bool";
        }
        public void ToString(StringBuilder builder)
        {
            builder.Append(name);
        }
        public string ToString()
        {
            return this.name;
        }
    }

    public partial class BoolStatement : Statement 
    {
        public bool value;
        public BoolStatement(string name) {
            this.value = Convert.ToBoolean(name);
        }
        public void ToString(StringBuilder builder)
        {
            builder.Append(value.ToString());
        }
    }

    public partial class NumStatement : Statement
    {
        public int num;

        public NumStatement(string num)
        {
            this.num = Convert.ToInt32(num);
            complexStatement = false;
        }

        public void ToString(StringBuilder builder)
        {
            builder.Append(num.ToString());
        }
    }

    public partial class Type : Statement
    {
        protected string name;

        public Type()
        {

        }
        public override string ToString()
        {
            return name;
        }
    }

}
