using System;

namespace SyntaxAnalyser.Exceptions
{
    public class MissingCurlyBraceClosedException : ParserException
    {
        public MissingCurlyBraceClosedException(int row, int col) : base($"CurlyBraceClosed token expected at row {row} column {col}")
        {
        }
    }
}