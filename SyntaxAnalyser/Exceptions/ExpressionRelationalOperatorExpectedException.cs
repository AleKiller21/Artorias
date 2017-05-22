namespace SyntaxAnalyser.Exceptions
{
    internal class ExpressionRelationalOperatorExpectedException : ParserException
    {
        public ExpressionRelationalOperatorExpectedException(int row, int col) : base($"Expression relational operator expected at row {row} column {col}")
        {
        }
    }
}