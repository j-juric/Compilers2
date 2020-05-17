using Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2
{
    public class ErrorMessage
    {
        public enum ErrorCode
        {
            UOP_1, // Expected value in unary - operator
            UOP_2, // Expected boolean in unary ! operator
            BOP_1, // Expected integers in arithmetic binary operator
            BOP_2, // Expected booleans in || operator
            BOP_3, // Expected booleans in && operator
            BOP_4, // Expected same value type in binary equality operators
            BOP_5, // Expected integers in binary inequality operators
            ID, // Variable not declared
            ASN_1, // Expected value in assignment
            ASN_2,
            ASN_3,
            CALL_1, // Function not defined
            CALL_2, // Wrong number of arguments in function call
            CALL_3,
            DECL, // Variable already defined
            IF, // Expecting boolean guard in if
            WHILE, // Expecting boolean guard in while
        }
        Dictionary<ErrorCode, string> errorDescription = new Dictionary<ErrorCode, string>()
        {
            { ErrorCode.UOP_1 , "Expected value in unary - operator" },
            { ErrorCode.UOP_2 , "Expected boolean in unary ! operator" },
            { ErrorCode.BOP_1 , "Expected integers in arithmetic binary operator" },
            { ErrorCode.BOP_2 , "Expected booleans in || operator" },
            { ErrorCode.BOP_3 , "Expected booleans in && operator" },
            { ErrorCode.BOP_4 , "Expected same value type in binary equality operators" },
            { ErrorCode.BOP_5 , "Expected integers in binary inequality operators" },
            { ErrorCode.ID , "Variable not declared" },
            { ErrorCode.ASN_1 , "Expected value in assignment" },
            { ErrorCode.ASN_2 , "Declared type not matching type of value in assignment" },
            { ErrorCode.ASN_3 , "Return value doesn't match function type" },
            { ErrorCode.CALL_1 , "Function not defined" },
            { ErrorCode.CALL_2 , "Wrong number of arguments in function call" },
            { ErrorCode.CALL_3 , "Wrong argument type in function call" },
            { ErrorCode.DECL , "Variable already defined" },
            { ErrorCode.IF , "Expecting boolean guard in if" },
            { ErrorCode.WHILE , "Expecting boolean guard in while" },
        };
        public virtual string ErrorOutput(ErrorCode ec, Locatable loc)
        {
            return $"INTERPRETATION ERROR: ({ec.ToString()}) - Line :{loc.line.ToString()}, Column:{loc.column.ToString()} | {errorDescription[ec]}";
        }
    }
}
