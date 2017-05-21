using System;

namespace SyntaxAnalyser.Exceptions
{
    public class MissingNamespaceKeywordException : ParserException
    {
        public MissingNamespaceKeywordException(int row, int col) : base($"namespace keyword expected at row {row} column {col}")
        {
        }
    }
}