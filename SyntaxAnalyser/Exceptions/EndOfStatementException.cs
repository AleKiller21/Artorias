using System;

namespace SyntaxAnalyser.Exceptions
{
    public class EndOfStatementException : ParserException
    {
        public EndOfStatementException(int row, int col) : base($"End of statement expected at row {row} column {col}")
        {
        }
    }
}