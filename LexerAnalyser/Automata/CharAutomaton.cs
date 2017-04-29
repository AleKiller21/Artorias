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
        private Token GetCharToken()
        {
            var lexeme = new StringBuilder("" + _currentSymbol.Character);
            var row = _currentSymbol.RowCount;
            var col = _currentSymbol.ColCount;
            var ctx = 'n';

            _currentSymbol = _inputStream.GetNextSymbol();
            if(_currentSymbol.Character == '\'') throw new LexicalCharException(String.Format("Empty character literal at row {0} column {1}.", row, col));

            if (_currentSymbol.Character == '\\') return GetEscapeSecuenceToken(lexeme, row, col);
            if (isValidCharacter(_currentSymbol.Character)) return GetSimpleCharToken(lexeme, row, col);

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
            throw new NotImplementedException();
        }

        private bool isValidCharacter(char symbol)
        {
            int value = symbol;

            return (value >= 32 && value <= 38) || (value >= 40 && value <= 126);
        }
    }
}
