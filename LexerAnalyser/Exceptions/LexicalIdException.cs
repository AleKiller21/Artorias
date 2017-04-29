using System;
using System.Collections.Generic;
using System.Text;

namespace LexerAnalyser.Exceptions
{
    public class LexicalIdException : LexicalException
    {
        private static string cause = "Identifier expected.";

        public LexicalIdException(int row, int col) : base(String.Format("Invalid Integer at row {0} column {1}. {2}", row, col, cause))
        {
        }
    }
}
