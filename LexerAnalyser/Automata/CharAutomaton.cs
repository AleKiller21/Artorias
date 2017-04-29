using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using LexerAnalyser.Exceptions;
using LexerAnalyser.Models;

namespace LexerAnalyser.Automata
{
    public partial class Automaton
    {
        private Dictionary<char, TokenType> _escapeSecuenceDictionary;

        private Token GetCharToken()
        {
            var lexeme = new StringBuilder("" + _currentSymbol.Character);
            var row = _currentSymbol.RowCount;
            var col = _currentSymbol.ColCount;

            _currentSymbol = _inputStream.GetNextSymbol();
            if(_currentSymbol.Character == '\'') throw new LexicalCharException(String.Format("Empty character literal at row {0} column {1}.", row, col));

            if (_currentSymbol.Character == '\\') return GetEscapeSecuenceToken(lexeme, row, col);
            if (IsValidCharacter(_currentSymbol.Character)) return GetSimpleCharToken(lexeme, row, col);

            throw new LexicalCharException(String.Format("Invalid char literal at row {0} column {1}.", row, col));
        }

        private Token GetSimpleCharToken(StringBuilder lexeme, int row, int col)
        {
            var message = String.Format(
                "A character literal can only have a single value surrounded by quotes. Row {0} column {1}.", row, col);

            lexeme.Append(_currentSymbol.Character);
            _currentSymbol = _inputStream.GetNextSymbol();
            if(_currentSymbol.Character != '\'') throw new LexicalCharException(message);

            lexeme.Append(_currentSymbol.Character);
            _currentSymbol = _inputStream.GetNextSymbol();
            return new Token(lexeme.ToString(), TokenType.CharSimple, row, col);
        }

        private Token GetEscapeSecuenceToken(StringBuilder lexeme, int row, int col)
        {
            TokenType type;
            var message = String.Format(
                "A character literal can only have a single value surrounded by quotes. Row {0} column {1}.", row, col);

            lexeme.Append(_currentSymbol.Character);
            _currentSymbol = _inputStream.GetNextSymbol();
            try
            {
                type = _escapeSecuenceDictionary[_currentSymbol.Character];
                lexeme.Append(_currentSymbol.Character);
            }
            catch (KeyNotFoundException e)
            {
                throw new LexicalCharException(String.Format("Unrecognized escape secuence at row {0} column {1}", row, col));
            }

            _currentSymbol = _inputStream.GetNextSymbol();
            if(_currentSymbol.Character != '\'') throw new LexicalCharException(message);

            lexeme.Append(_currentSymbol.Character);
            _currentSymbol = _inputStream.GetNextSymbol();

            return new Token(lexeme.ToString(), type, row, col);
        }

        private bool IsValidCharacter(char symbol)
        {
            int value = symbol;

            return (value >= 32 && value <= 38) || (value >= 40 && value <= 126);
        }

        private void InitEscapeSecuenceDictionary()
        {
            _escapeSecuenceDictionary = new Dictionary<char, TokenType>();
            _escapeSecuenceDictionary['\''] = TokenType.EscapeSecuenceSingleQuote;
            _escapeSecuenceDictionary['\"'] = TokenType.EscapeSecuenceDoubleQuote;
            _escapeSecuenceDictionary['\\'] = TokenType.EscapeSecuenceBackslash;
            _escapeSecuenceDictionary['0'] = TokenType.EscapeSecuenceZero;
            _escapeSecuenceDictionary['a'] = TokenType.EscapeSecuenceAlert;
            _escapeSecuenceDictionary['b'] = TokenType.EscapeSecuenceBackspace;
            _escapeSecuenceDictionary['f'] = TokenType.EscapeSecuenceFormFeed;
            _escapeSecuenceDictionary['n'] = TokenType.EscapeSecuenceNewLine;
            _escapeSecuenceDictionary['r'] = TokenType.EscapeSecuenceCarriageReturn;
            _escapeSecuenceDictionary['t'] = TokenType.EscapeSecuenceHorizontalTab;
            _escapeSecuenceDictionary['v'] = TokenType.EscapeSecuenceVerticalQuote;
        }
    }
}
