using System;

namespace SyntaxAnalyser.Exceptions
{
    public class MissingCurlyBraceOpenException : Exception
    {
        public MissingCurlyBraceOpenException(string message) : base(message)
        {
        }
    }
}