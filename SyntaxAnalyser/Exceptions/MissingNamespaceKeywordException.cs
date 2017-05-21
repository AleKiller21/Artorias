using System;

namespace SyntaxAnalyser.Exceptions
{
    public class MissingNamespaceKeywordException : Exception
    {
        public MissingNamespaceKeywordException(int row, int col) : base($"namespace keyword expected at row {row} column {col}")
        {
        }
    }
}