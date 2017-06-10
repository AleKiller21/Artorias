using System;

namespace SyntaxAnalyser.Exceptions
{
    public class ThisKeywordExpectedException : Exception
    {
        public ThisKeywordExpectedException(int row, int col) : base($"'this' keyword expected at row {row} column {col}.")
        {
        }
    }
}