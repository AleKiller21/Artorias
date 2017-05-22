namespace SyntaxAnalyser.Exceptions
{
    internal class ExpressionShiftOperatorExpectedException : ParserException
    {
        public ExpressionShiftOperatorExpectedException(int row, int col) : base($"Expression shift operator expected at row {row} column {col}")
        {
        }
    }
}