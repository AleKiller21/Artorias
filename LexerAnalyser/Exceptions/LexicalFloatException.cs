using System;
using System.Collections.Generic;
using System.Text;

namespace LexerAnalyser.Exceptions
{
    class LexicalFloatException : LexicalException
    {
        private static string cause = "Invalid float literal.";

        public LexicalFloatException(int row, int col) : base(row, col, cause)
        {
            
        }
    }
}
