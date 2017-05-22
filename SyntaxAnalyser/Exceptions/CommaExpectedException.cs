namespace SyntaxAnalyser.Exceptions
{
    internal class CommaExpectedException : ParserException
    {
        public CommaExpectedException(int row, int col) : base($"Comma token expected at row {row} column {col}.")
        {
        }
    }
}