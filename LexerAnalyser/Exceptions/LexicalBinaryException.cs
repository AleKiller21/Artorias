using System;
using System.Collections.Generic;
using System.Text;

namespace LexerAnalyser.Exceptions
{
    public class LexicalBinaryException : LexicalException
    {
        private static string cause = "Invalid binary literal.";
        public LexicalBinaryException(int row, int col) : base(String.Format("{0} at row {1} column {2}", cause, row, col))
        {

        }
    }
}
