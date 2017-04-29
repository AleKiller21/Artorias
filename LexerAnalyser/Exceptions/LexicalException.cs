using System;

namespace LexerAnalyser.Exceptions
{
    public class LexicalException : Exception
    {
        public LexicalException(string cause) : base(cause)
        {
        }
    }
}
