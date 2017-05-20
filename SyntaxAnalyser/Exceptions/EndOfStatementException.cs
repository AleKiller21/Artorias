using System;

namespace SyntaxAnalyser.Exceptions
{
    public class EndOfStatementException : Exception
    {
        public EndOfStatementException(string message) : base(message)
        {
        }
    }
}