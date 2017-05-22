namespace SyntaxAnalyser.Exceptions
{
    internal class SquareBracketCloseExpectedException : ParserException
    {
        public SquareBracketCloseExpectedException(int row, int col) : base($"']' token expected at row {row} column {col}")
        {
        }
    }
}