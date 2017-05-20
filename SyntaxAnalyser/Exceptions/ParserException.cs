using System;

namespace SyntaxAnalyser.Exceptions
{
    public class ParserException : Exception
    {
        public ParserException(string message) : base(message)
        {
        }
    }
}
