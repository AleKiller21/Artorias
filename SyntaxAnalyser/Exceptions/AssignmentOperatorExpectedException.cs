namespace SyntaxAnalyser.Exceptions
{
    public class AssignmentOperatorExpectedException : ParserException
    {
        public AssignmentOperatorExpectedException(int row, int col) : base((string) $"Assignment operator expected at row {row} column {col}")
        {
        }
    }
}