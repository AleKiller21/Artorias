using System;

namespace SyntaxAnalyser.Exceptions
{
    internal class MissingDataTypeForIdentifierToken : ParserException
    {
        public MissingDataTypeForIdentifierToken(int row, int col) : base($"Data type or void expected at row {row} column {col}")
        {
        }
    }
}