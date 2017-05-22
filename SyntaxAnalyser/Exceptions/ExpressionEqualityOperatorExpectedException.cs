namespace SyntaxAnalyser.Exceptions
{
    internal class ExpressionEqualityOperatorExpectedException : ParserException
    {
        public ExpressionEqualityOperatorExpectedException(int row, int col) : base($"Expression equality operator expected at row{row} column {col}")
        {
        }
    }
}