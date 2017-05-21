using System;

namespace SyntaxAnalyser.Exceptions
{
    public class MissingInterfaceKeywordException : ParserException
    {
        public MissingInterfaceKeywordException(int row, int col) : base($"inteface keyword expected at row {row} column {col}")
        {
        }
    }
}
