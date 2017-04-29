using System;

namespace LexerAnalyser.Exceptions
{
    public class LexicalException : Exception
    {
        public LexicalException(int row, int col, string cause) : base(String.Format("Invalid Integer at row {0} column {1}. {2}", row, col, cause))
        {
        }
    }
}
