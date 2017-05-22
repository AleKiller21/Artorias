using System;

namespace SyntaxAnalyser.Exceptions
{
    internal class IncrementDecrementOperatorExpectedException : Exception
    {
        public IncrementDecrementOperatorExpectedException(int row, int col) : base($"++ or -- operator expected at row {row} column {col}")
        {
        }
    }
}