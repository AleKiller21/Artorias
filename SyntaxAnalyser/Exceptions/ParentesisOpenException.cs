using System;

namespace SyntaxAnalyser.Exceptions
{
    public class ParentesisOpenException : ParserException
    {
        public ParentesisOpenException(int row, int col) : base((string) $"ParenthesisOpen token expected at row {row} column {col}")
        {
        }
    }
}