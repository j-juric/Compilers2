using Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using Type = Syntax.Type;
namespace Lab2
{

    public class TypeEnvironment
    {
        public Dictionary<string, Type> VariableType = new Dictionary<string, Type>();
        public Dictionary<string, Declaration> FunctionType = new Dictionary<string, Declaration>();

    }

    public class TypeError : Exception
    {
        public TypeError(string msg) : base(msg)
        {
        }
    }

    public class TypeCheckerVisitor : IExpressionVisitor<Type>
    {
        TypeEnvironment Environment = new TypeEnvironment();

        bool IsVoidType(string type_text)
        {
            return (type_text != "IntType" && type_text != "BoolType");
        }

        

        public Type Visit(DeclarationSequence declarationSequence)
        {
            var declaration = declarationSequence;
            while (declaration != null)
            {
                var typeName = declaration.tail.GetType().Name;

                if (typeName == "VoidDeclaration")
                    Environment.FunctionType.Add(((VoidDeclaration)declaration.tail).id, ((VoidDeclaration)declaration.tail));
                else
                    Environment.FunctionType.Add(((TypeDeclaration)declaration.tail).id, ((TypeDeclaration)declaration.tail));
                declaration = (DeclarationSequence)declaration.head;

            }

            declaration = declarationSequence;

            while (declaration != null)
            {
                ((VoidDeclaration)declaration.tail).Accept(this);
                declaration = (DeclarationSequence)declaration.head;
            }

            return null;
        }

        public Type Visit(SequenceStatement sequenceStatement)
        {
            if (sequenceStatement.head != null)
            {
                ((SequenceStatement)sequenceStatement.head).Accept(this);
            }
            sequenceStatement.tail.Accept(this);
            return null;
        }

        public Type Visit(IdentifierStatement identifierStatement)
        {
            throw new NotImplementedException();
        }

        public Type Visit(NumStatement numStatement)
        {
            throw new NotImplementedException();
        }

        public Type Visit(BlockStatement blockStatement)
        {
            return null;
        }

        public Type Visit(BinOperatorStatement binOperatorStatement)
        {
            throw new NotImplementedException();
        }

        public Type Visit(ListStatement listStatement)
        {
            throw new NotImplementedException();
        }

        public Type Visit(FormalList formalList)
        {
            throw new NotImplementedException();
        }

        public Type Visit(Declaration declaration)
        {
            throw new NotImplementedException();
        }

        public Type Visit(Argument argument)
        {
            if (Environment.VariableType.ContainsKey(argument.id))
            {
                throw new TypeError("Error");
            }
            Environment.VariableType.Add(argument.id, (Type)argument.type);
            return null;
        }

        public Type Visit(TypeDeclaration typeDeclaration)
        {
            throw new NotImplementedException();
        }

        public Type Visit(VoidDeclaration voidDeclaration)
        {
            throw new NotImplementedException();
        }

        public Type Visit(IfStatement ifStatement)
        {
            throw new NotImplementedException();
        }

        public Type Visit(IfElseStatement ifElseStatement)
        {
            throw new NotImplementedException();
        }

        public Type Visit(WhileStatement whileStatement)
        {
            throw new NotImplementedException();
        }

        public Type Visit(RegularStatement regularStatement)
        {
            return null;
        }

        public Type Visit(Return rreturn)
        {
            throw new NotImplementedException();
        }

        public Type Visit(VoidReturn voidReturn)
        {
            throw new NotImplementedException();
        }

        public Type Visit(AssignStatement assignStatement)
        {
            if (!Environment.VariableType.ContainsKey(assignStatement.id))
            {
                throw new TypeError("ERROR");
            }
            var type = Environment.VariableType[assignStatement.id].Accept(this);
            var type_text = type.GetType().Name;

            var result_type_text = assignStatement.Accept(this).GetType().Name;

            if (IsVoidType(result_type_text))
            {
                throw new TypeError("Error");
            }
            if(result_type_text != type_text)
            {
                throw new TypeError("Error");
            }

            return type;

        }

        public Type Visit(NotStatement notStatement)
        {
            var type = notStatement.s1.Accept(this);
            var type_text = type.GetType().Name;
            if (IsVoidType(type_text))
            {
                throw new TypeError("Error");
            }
            if (type_text != "BoolType")
            {
                throw new TypeError("Error");
            }
            return type;
        }

        public Type Visit(NegativeStatement negativeStatement)
        {
            var type = negativeStatement.s1.Accept(this);
            var type_text = type.GetType().Name;
            if (IsVoidType(type_text))
            {
                throw new TypeError("Error");
            }
            if (type_text != "IntType")
            {
                throw new TypeError("Error");
            }
            return type;
        }

        public Type Visit(IntType intType)
        {
            return new IntType();
        }

        public Type Visit(BoolType boolType)
        {
            return new BoolType();
        }

        public Type Visit(BoolStatement boolStatement)
        {
            return null;
        }

        public Type Visit(Type type)
        {
            return new Type();
        }
    }
}
