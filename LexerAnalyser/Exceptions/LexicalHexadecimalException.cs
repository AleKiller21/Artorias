using System;
using System.Collections.Generic;
using System.Text;

namespace LexerAnalyser.Exceptions
{
    public class LexicalHexadecimalException : LexicalException
    {
        private static string cause = "Expected a hexadecimal literal.";
        public LexicalHexadecimalException(int row, int col) : base(row, col, cause)
        {
        }
    }
}
