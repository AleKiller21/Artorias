namespace SyntaxAnalyser.Exceptions
{
    internal class IsAsOperatorExpectedException : ParserException
    {
        public IsAsOperatorExpectedException(int row, int col) : base($"is or as operators expected at row {row} column {col}")
        {
        }
    }
}