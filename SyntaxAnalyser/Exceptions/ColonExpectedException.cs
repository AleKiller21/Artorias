using System;

namespace SyntaxAnalyser.Exceptions
{
    internal class ColonExpectedException : Exception
    {
        public ColonExpectedException(int row, int col) : base($"Colon token ':' expected at row {row} column {col}")
        {
        }
    }
}