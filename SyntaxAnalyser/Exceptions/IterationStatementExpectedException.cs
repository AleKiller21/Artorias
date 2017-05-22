namespace SyntaxAnalyser.Exceptions
{
    public class IterationStatementExpectedException : ParserException
    {
        public IterationStatementExpectedException(int row, int col) : base($"'while', 'do', 'for', or 'foreach' token expected at row {row} column {col}.")
        {
        }
    }
}
