using System;

namespace SyntaxAnalyser.Exceptions
{
    public class MissingUsingKeywordException : Exception
    {
        public MissingUsingKeywordException(string message) : base(message)
        {
        }
    }
}