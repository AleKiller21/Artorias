using System;

namespace SyntaxAnalyser.Exceptions
{
    public class IdTokenExpectecException : Exception
    {
        public IdTokenExpectecException(int row, int col) : base($"identifier token expected at rowc {row} column {col}")
        {
        }
    }
}