namespace SyntaxAnalyser.Exceptions
{
    public class ClassKeywordExpectedException : ParserException
    {
        public ClassKeywordExpectedException(int row, int col) : base($"Class keyword expected at row {row} column {col}")
        {
            
        }
    }
}
