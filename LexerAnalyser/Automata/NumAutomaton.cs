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
                //lexeme.Append(_currentSymbol.Character);
                //_currentSymbol = _inputStream.GetNextSymbol();
                ConsumeSymbol(lexeme);

                if (_currentSymbol.Character != 'f' && _currentSymbol.Character != 'F') continue;
                ConsumeSymbol(lexeme);
                return new Token(lexeme.ToString(), TokenType.LiteralFloat, rowCount, colCount);
            }

            if (_currentSymbol.Character == '.')
            {
                if(Char.IsDigit(_inputStream.PeekNextSymbol().Character))
                    return GetFloatToken(lexeme, rowCount, colCount);
            }

            return new Token(lexeme.ToString(), TokenType.LiteralSimpleNum, rowCount, colCount);
        }

        private Token GetFloatToken(StringBuilder lexeme, int row, int col)
        {
            do
            {
                lexeme.Append(_currentSymbol.Character);
                _currentSymbol = _inputStream.GetNextSymbol();
            } while (Char.IsDigit(_currentSymbol.Character));

            if(!(_currentSymbol.Character == 'f' || _currentSymbol.Character == 'F')) throw new LexicalFloatException(row, col);

            lexeme.Append(_currentSymbol.Character);
            _currentSymbol = _inputStream.GetNextSymbol();
            return new Token(lexeme.ToString(), TokenType.LiteralFloat, row, col);
        }

        private Token GetNumWithPrefixToken()
        {
            var lexeme = new StringBuilder("" + _currentSymbol.Character);
            var rowCount = _currentSymbol.RowCount;
            var colCount = _currentSymbol.ColCount;
            var symbol = _inputStream.GetNextSymbol();

            if (Char.IsDigit(symbol.Character) || '.'.Equals(symbol.Character))
            {
                //TODO Esto devolvera todo float que empieze con 0 sin el 0. Ejemplo, 0.5f lo devolvera como .5f
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
                throw new LexicalBinaryException(rowCount, colCount);

            while (_currentSymbol.Character == '0' || _currentSymbol.Character == '1')
            {
                lexeme.Append(_currentSymbol.Character);
                _currentSymbol = _inputStream.GetNextSymbol();
            }

            return new Token(lexeme.ToString(), TokenType.LiteralBinary, rowCount,colCount);
        }

        private Token GetHexadecimalToken(Symbol symbol)
        {
            var lexeme = new StringBuilder("" + _currentSymbol.Character + symbol.Character);
            var rowCount = _currentSymbol.RowCount;
            var colCount = _currentSymbol.ColCount;

            _currentSymbol = _inputStream.GetNextSymbol();
            if(!IsCharacterInHexadecimalRange(_currentSymbol.Character))
                throw new LexicalHexadecimalException(rowCount, colCount);

            do
            {
                lexeme.Append(_currentSymbol.Character);
                _currentSymbol = _inputStream.GetNextSymbol();
            } while (IsCharacterInHexadecimalRange(_currentSymbol.Character));

            return new Token(lexeme.ToString(), TokenType.LiteralHexadecimal, rowCount, colCount);
        }

        private bool IsCharacterInHexadecimalRange(char symbol)
        {
            var numericValue = symbol;

            return (numericValue >= 48 && numericValue <= 57) ||
                   (numericValue >= 65 && numericValue <= 70) ||
                   (numericValue >= 97 && numericValue <= 102);
        }
    }
}
