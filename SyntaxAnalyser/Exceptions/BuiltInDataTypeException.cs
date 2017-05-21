namespace SyntaxAnalyser.Exceptions
{
    internal class BuiltInDataTypeException : ParserException
    {
        public BuiltInDataTypeException(int row, int col) : base($"Built in data type expected at row {row} column {col}")
        {
        }
    }
}