using System;
using System.Collections.Generic;
using System.Text;

namespace LexerAnalyser.Exceptions
{
    public class LexicalCharException : LexicalException
    {
        public LexicalCharException(string message) : base(message)
        {
        }
    }
}
