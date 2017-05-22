namespace SyntaxAnalyser.Exceptions
{
    internal class AdditiveOperatorExpectedException : ParserException
    {
        public AdditiveOperatorExpectedException(int row, int col) : base($"Additive operator expected exception in row {row} column {col}")
        {
        }
    }
}