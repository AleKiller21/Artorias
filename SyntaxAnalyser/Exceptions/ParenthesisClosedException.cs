using System;

namespace SyntaxAnalyser.Exceptions
{
    public class ParenthesisClosedException : Exception
    {
        public ParenthesisClosedException(int row, int col) : base($"ParenthesisClosed token expected at row {row} column {col}")
        {
            
        }
    }
}
