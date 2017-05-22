namespace SyntaxAnalyser.Exceptions
{
    internal class InKeywordExpectedException : ParserException
    {
        public InKeywordExpectedException(int row, int col) : base($"'in' keyword expected at row {row} column {col}")
        {
        }
    }
}