using System;

namespace SyntaxAnalyser.Exceptions
{
    public class NewKeywordExpectedException : Exception
    {
        public NewKeywordExpectedException(int row, int col) : base($"'new' keyword expected at row {row} column {col}.")
        {
        }
    }
}