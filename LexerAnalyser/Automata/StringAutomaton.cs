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
        private Token GetStringToken()
        {
            var lexeme = new StringBuilder("" + _currentSymbol.Character);
            var row = _currentSymbol.RowCount;
            var col = _currentSymbol.ColCount;

            _currentSymbol = _inputStream.GetNextSymbol();
            if(_currentSymbol.Character == '\"')
                return new Token(lexeme.Append(_currentSymbol.Character).ToString(), TokenType.LiteralRegularString, row, col);

            return GetRegularStringToken(lexeme, row, col);
        }

        private void ConsumeEscapeSecuenceChar(StringBuilder lexeme)
        {
            lexeme.Append(_currentSymbol.Character);
            _currentSymbol = _inputStream.GetNextSymbol();
            try
            {
                var type = _escapeSecuenceDictionary[_currentSymbol.Character];
            }
            catch (KeyNotFoundException e)
            {
                throw new LexicalCharException(String.Format("Unrecognized escape secuence at row {0} column {1}", _currentSymbol.RowCount, _currentSymbol.ColCount));
            }
        }

        private Token GetRegularStringToken(StringBuilder lexeme, int row, int col)
        {
            //var x = "\"";
            var message = String.Format("Invalid string literal at row {0} column {1}.", row, col);
            while (IsValidStringCharacter() || IsCurrentSymbolBackSlash())
            {
                if (IsCurrentSymbolBackSlash()) ConsumeEscapeSecuenceChar(lexeme);

                lexeme.Append(_currentSymbol.Character);
                _currentSymbol = _inputStream.GetNextSymbol();
            }

            if(_currentSymbol.Character != '\"') throw new LexicalException(message);

            lexeme.Append(_currentSymbol.Character);
            _currentSymbol = _inputStream.GetNextSymbol();
            return new Token(lexeme.ToString(), TokenType.LiteralRegularString, row, col);
        }

        private bool IsValidStringCharacter()
        {
            int value = _currentSymbol.Character;

            return value == 9 || (value == 32 || value == 33) || 
                    (value >= 35 && value <= 91) || 
                    (value >= 93 && value <= 126);
        }
    }
}
