using System;
using System.Collections.Generic;
using System.Text;

namespace LexerAnalyser.Exceptions
{
    public class LexicalHexadecimalException : LexicalException
    {
        private static string cause = "Invalid hexadecimal literal.";
        public LexicalHexadecimalException(int row, int col) : base(String.Format("{0} at row {1} column {2}", cause, row, col))
        {
        }
    }
}
