using System;

namespace SyntaxAnalyser.Exceptions
{
    public class MissingUsingKeywordException : ParserException
    {
        public MissingUsingKeywordException(int row, int col) : base($"using keyword expected at row {row} column {col}")
        {
        }
    }
}