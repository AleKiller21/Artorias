namespace SyntaxAnalyser.Exceptions
{
    public class BaseKeywordExpectedException : ParserException
    {
        public BaseKeywordExpectedException(int row, int col) : base($"base keyword expected at rorw {row} column {col}")
        {
        }
    }
}
