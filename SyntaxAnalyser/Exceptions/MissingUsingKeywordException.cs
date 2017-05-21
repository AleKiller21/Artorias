using System;

namespace SyntaxAnalyser.Exceptions
{
    public class MissingUsingKeywordException : Exception
    {
        public MissingUsingKeywordException(int row, int col) : base($"using keyword expected at row {row} column {col}")
        {
        }
    }
}