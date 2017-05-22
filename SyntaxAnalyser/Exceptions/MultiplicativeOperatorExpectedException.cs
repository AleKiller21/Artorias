namespace SyntaxAnalyser.Exceptions
{
    internal class MultiplicativeOperatorExpectedException : ParserException
    {
        public MultiplicativeOperatorExpectedException(int row, int col) : base($"Multiplicative operator expected at row {row} column {col}")
        {
        }
    }
}