using System;
using System.Collections.Generic;
using System.Text;

namespace LexerAnalyser
{
    public class LexicalException : Exception
    {
        public LexicalException(string message) : base(message)
        {
        }
    }
}
