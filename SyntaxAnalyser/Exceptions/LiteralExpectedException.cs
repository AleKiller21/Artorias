namespace SyntaxAnalyser.Exceptions
{
    internal class LiteralExpectedException : ParserException
    {
        public LiteralExpectedException(int row, int col) : base($"Literal token expected at row {row} column {col}")
        {
        }
    }
}