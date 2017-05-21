using System;

namespace SyntaxAnalyser.Exceptions
{
    public class MissingCurlyBraceClosedException : Exception
    {
        public MissingCurlyBraceClosedException(int row, int col) : base($"CurlyBraceClosed token expected at row {row} column {col}")
        {
        }
    }
}