using System;

namespace SyntaxAnalyser.Exceptions
{
    internal class MissingDataTypeForIdentifierToken : ParserException
    {
        public MissingDataTypeForIdentifierToken(int row, int col) : base($"Data type expected at row {row} column {col}")
        {
        }
    }
}