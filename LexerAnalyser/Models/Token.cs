using LexerAnalyser.Enums;

namespace LexerAnalyser.Models
{
    public class Token
    {
        public readonly string Lexeme;
        public readonly TokenType Type;
        public readonly int Row;
        public readonly int Column;

        public Token(string lexeme, TokenType type, int row, int column)
        {
            Lexeme = lexeme;
            Type = type;
            Row = row;
            Column = column;
        }
    }
}
