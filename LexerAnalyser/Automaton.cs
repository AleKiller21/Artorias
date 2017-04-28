using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LexerAnalyser.Enums;
using LexerAnalyser.Interfaces;
using LexerAnalyser.Models;

namespace LexerAnalyser
{
    public class Automaton
    {
        private readonly IInputStream _inputStream;

        public Automaton(IInputStream inputStream)
        {
            _inputStream = inputStream;
        }

        public Token GetToken()
        {
            var symbol = _inputStream.GetNextSymbol();
            while (symbol.Character != '\0')
            {
                if (Char.IsLetter(symbol.Character)) return GetIdToken(symbol);

                symbol = _inputStream.GetNextSymbol();
            }

            return new Token("\0", TokenType.Eof, symbol.RowCount, symbol.ColCount);
        }

        private Token GetIdToken(Symbol symbol)
        {
            var lexeme = new StringBuilder(symbol.Character.ToString());
            var rowCount = symbol.RowCount;
            var colCount = symbol.ColCount;

            symbol = _inputStream.GetNextSymbol();
            while (Char.IsLetterOrDigit(symbol.Character))
            {
                lexeme.Append(symbol.Character);
                symbol = _inputStream.GetNextSymbol();
            }

            return new Token(lexeme.ToString(), TokenType.Id, rowCount, colCount);
        }
    }
}
