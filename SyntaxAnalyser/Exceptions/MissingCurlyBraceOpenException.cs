using System;

namespace SyntaxAnalyser.Exceptions
{
    public class MissingCurlyBraceOpenException : ParserException
    {
        public MissingCurlyBraceOpenException(int row, int col) : base($"OpenCurlyBrace token expected at row {row} column {col}")
        {
        }
    }
}