namespace SyntaxAnalyser.Exceptions
{
    internal class SquareBracketOpenExpectedException : ParserException
    {
        public SquareBracketOpenExpectedException(int row, int col) : base($"'[' token expected at row {row} column {col}")
        {
        }
    }
}