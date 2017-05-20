using System;

namespace SyntaxAnalyser.Exceptions
{
    public class MissingCurlyBraceClosedException : Exception
    {
        public MissingCurlyBraceClosedException(string message) : base(message)
        {
        }
    }
}