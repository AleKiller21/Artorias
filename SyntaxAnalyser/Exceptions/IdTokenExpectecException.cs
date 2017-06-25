using System;

namespace SyntaxAnalyser.Exceptions
{
    public class IdTokenExpectecException : ParserException
    {
        public IdTokenExpectecException(int row, int col) : base($"identifier token expected at row {row} column {col}")
        {
        }
    }
}