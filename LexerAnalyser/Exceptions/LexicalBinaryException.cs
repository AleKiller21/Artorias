using System;
using System.Collections.Generic;
using System.Text;

namespace LexerAnalyser.Exceptions
{
    public class LexicalBinaryException : LexicalException
    {
        private static string cause = "Expected a binary literal.";
        public LexicalBinaryException(int row, int col) : base(row, col, cause)
        {

        }
    }
}
