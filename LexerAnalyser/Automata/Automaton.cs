using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using LexerAnalyser.Interfaces;
using LexerAnalyser.Models;

namespace LexerAnalyser.Automata
{
    public partial class Automaton
    {
        private readonly IInputStream _inputStream;
        private Dictionary<string, TokenType> _operatorsDictionary;
        private Dictionary<char, TokenType> _punctuatorsDictionary;
        private Symbol _currentSymbol;

        public Automaton(IInputStream inputStream)
        {
            _inputStream = inputStream;
            _currentSymbol = _inputStream.GetNextSymbol();
            InitEscapeSecuenceDictionary();
            InitializePunctuatorsDictionary();
            InitializeOperatorsDictionary();
        }

        public Token GetToken()
        {
            while (_currentSymbol.Character != '\0')
            {
                if (Char.IsLetter(_currentSymbol.Character)) return GetOpenToken();
                if (Char.IsDigit(_currentSymbol.Character)) return GetNumLiteralToken();
                if (_currentSymbol.Character == '\'') return GetCharToken();
                if (_currentSymbol.Character == '\"') return GetStringToken();
                if (_currentSymbol.Character == '@') return GetVerbatimStringToken();

                var token = GetPunctuatorToken();
                if (token != null) return token;

                token = GetOperatorToken();
                if (token != null) return token;

                _currentSymbol = _inputStream.GetNextSymbol();
            }

            return new Token("\0", TokenType.Eof, _currentSymbol.RowCount, _currentSymbol.ColCount);
        }

        private Token GetOpenToken()
        {
            Token token = GetIdToken();

            //TODO soportar null literal
            if(token.Lexeme.Equals("true")) return new Token(token.Lexeme, TokenType.LiteralTrue, token.Row, token.Column);
            if(token.Lexeme.Equals("false")) return new Token(token.Lexeme, TokenType.LiteralFalse, token.Row, token.Column);

            return token;
        }

        private Token GetIdToken()
        {
            //TODO validar si el id es as o is los cuales son operadores
            //TODO Aceptar Unicode characters (opcional)
            //TODO permitir que el id pueda empezar con _
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

        private Token GetPunctuatorToken()
        {
            try
            {
                var type = _punctuatorsDictionary[_currentSymbol.Character];
                var row = _currentSymbol.RowCount;
                var col = _currentSymbol.ColCount;
                var lexeme = _currentSymbol.Character;

                _currentSymbol = _inputStream.GetNextSymbol();
                return new Token(lexeme.ToString(), type, row, col);
            }
            catch (KeyNotFoundException e)
            {
                return null;
            }
        }

        private void ConsumeSymbol(StringBuilder lexeme)
        {
            lexeme.Append(_currentSymbol.Character);
            _currentSymbol = _inputStream.GetNextSymbol();
        }

        private void InitializePunctuatorsDictionary()
        {
            _punctuatorsDictionary = new Dictionary<char, TokenType>
            {
                ['{'] = TokenType.CurlyBraceOpen,
                ['}'] = TokenType.CurlyBraceClose,
                ['['] = TokenType.SquareBracketOpen,
                [']'] = TokenType.SquareBracketClose,
                ['('] = TokenType.ParenthesisOpen,
                [')'] = TokenType.ParenthesisClose,
                ['.'] = TokenType.MemberAccess,
                [','] = TokenType.Comma,
                [':'] = TokenType.Colon,
                [';'] = TokenType.EndStatement
            };

        }
    }
}
