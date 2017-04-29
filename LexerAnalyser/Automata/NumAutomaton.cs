using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using LexerAnalyser.Models;

namespace LexerAnalyser.Automata
{
    public partial class Automaton
    {
        private Token GetNumLiteralToken()
        {
            return _currentSymbol.Character == '0' ? GetNumWithPrefixToken() : GetSimpleNumToken();
        }

        private Token GetSimpleNumToken()
        {
            var lexeme = new StringBuilder();
            var rowCount = _currentSymbol.RowCount;
            var colCount = _currentSymbol.ColCount;

            while (Char.IsDigit(_currentSymbol.Character))
            {
                lexeme.Append(_currentSymbol.Character);
                _currentSymbol = _inputStream.GetNextSymbol();
            }

            return new Token(lexeme.ToString(), TokenType.LiteralSimpleNum, rowCount, colCount);
        }

        private Token GetNumWithPrefixToken()
        {
            var lexeme = new StringBuilder("" + _currentSymbol.Character);
            var rowCount = _currentSymbol.RowCount;
            var colCount = _currentSymbol.ColCount;
            var symbol = _inputStream.GetNextSymbol();

            if (Char.IsDigit(symbol.Character))
            {
                _currentSymbol = symbol;

                //BUG Eliminar todos los leading zeros dejando solo el num o id token final
                //while (symbol.Character == '0') symbol = _inputStream.GetNextSymbol();
                //if (Char.IsDigit(symbol.Character)) _currentSymbol = symbol;
                return GetSimpleNumToken();
            }

            if (symbol.Character == 'x' || symbol.Character == 'X') return GetHexadecimalToken(symbol);
            if (symbol.Character == 'b' || symbol.Character == 'B') return GetBinaryToken(symbol);

            _currentSymbol = symbol;
            return new Token(lexeme.ToString(), TokenType.LiteralSimpleNum, rowCount, colCount);
        }

        private Token GetBinaryToken(Symbol symbol)
        {
            var lexeme = new StringBuilder("" + _currentSymbol.Character + symbol.Character);
            var rowCount = _currentSymbol.RowCount;
            var colCount = _currentSymbol.ColCount;

            _currentSymbol = _inputStream.GetNextSymbol();
            if (_currentSymbol.Character != '0' && _currentSymbol.Character != '1')
                throw new LexicalException("Invalid integer literal at row " + rowCount + " column " + colCount);

            while (_currentSymbol.Character == '0' || _currentSymbol.Character == '1')
            {
                lexeme.Append(_currentSymbol.Character);
                _currentSymbol = _inputStream.GetNextSymbol();
            }

            return new Token(lexeme.ToString(), TokenType.LiteralBinary, rowCount,colCount);
        }

        private Token GetHexadecimalToken(Symbol symbol)
        {


            throw new NotImplementedException();
        }
    }
}
