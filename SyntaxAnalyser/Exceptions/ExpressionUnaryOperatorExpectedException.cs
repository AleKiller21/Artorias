namespace SyntaxAnalyser.Exceptions
{
    internal class ExpressionUnaryOperatorExpectedException : ParserException
    {
        public ExpressionUnaryOperatorExpectedException(int row, int col) : base($"Expression unary operator expected at row {row} column {col}")
        {
        }
    }
}