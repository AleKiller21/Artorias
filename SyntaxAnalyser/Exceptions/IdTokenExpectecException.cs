using System;

namespace SyntaxAnalyser.Exceptions
{
    public class IdTokenExpectecException : Exception
    {
        public IdTokenExpectecException(string message) : base(message)
        {
        }
    }
}