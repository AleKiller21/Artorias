using System;

namespace SyntaxAnalyser.Exceptions
{
    internal class MissingEnumKeywordException : ParserException
    {
        public MissingEnumKeywordException(int row, int col) : base($"Missing enum keyword at row {row} column {col}")
        {
        }
    }
}