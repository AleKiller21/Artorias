using System;
using System.Text;
using LexerAnalyser.Enums;
using LexerAnalyser.Interfaces;
using LexerAnalyser.Models;

namespace LexerAnalyser.Automata
{
    public partial class Automaton
    {
        private readonly IInputStream _inputStream;
        private Symbol _currentSymbol;

        public Automaton(IInputStream inputStream)
        {
            _inputStream = inputStream;
            _currentSymbol = _inputStream.GetNextSymbol();
            InitEscapeSecuenceDictionary();
        }

        public Token GetToken()
        {
            while (_currentSymbol.Character != '\0')
            {
                if (Char.IsLetter(_currentSymbol.Character)) return GetOpenToken();
                if (Char.IsDigit(_currentSymbol.Character)) return GetNumLiteralToken();
                if (_currentSymbol.Character == '\'') return GetCharToken();
                if (_currentSymbol.Character == '\"') return GetStringToken();

                _currentSymbol = _inputStream.GetNextSymbol();
            }

            return new Token("\0", TokenType.Eof, _currentSymbol.RowCount, _currentSymbol.ColCount);
        }

        private Token GetOpenToken()
        {
            Token token = GetIdToken();

            if(token.Lexeme.Equals("true")) return new Token(token.Lexeme, TokenType.LiteralTrue, token.Row, token.Column);
            if(token.Lexeme.Equals("false")) return new Token(token.Lexeme, TokenType.LiteralFalse, token.Row, token.Column);

            return token;
        }

        private Token GetIdToken()
        {
            //TODO validar si el id formado no es una palabra reservada.
            //BUG tirar una excepcion cuando se ingrese caracteres no permitidos como \
            var lexeme = new StringBuilder();
            var rowCount = _currentSymbol.RowCount;
            var colCount = _currentSymbol.ColCount;
            
            while (Char.IsLetterOrDigit(_currentSymbol.Character))
            {
                lexeme.Append(_currentSymbol.Character);
                _currentSymbol = _inputStream.GetNextSymbol();
            }

            return new Token(lexeme.ToString(), TokenType.Id, rowCount, colCount);
        }
    }
}
