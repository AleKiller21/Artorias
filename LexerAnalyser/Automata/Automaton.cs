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
        }

        public Token GetToken()
        {
            while (_currentSymbol.Character != '\0')
            {
                if (Char.IsLetter(_currentSymbol.Character)) return GetIdToken();
                if (Char.IsDigit(_currentSymbol.Character)) return GetNumLiteralToken();

                _currentSymbol = _inputStream.GetNextSymbol();
            }

            return new Token("\0", TokenType.Eof, _currentSymbol.RowCount, _currentSymbol.ColCount);
        }

        private Token GetIdToken()
        {
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
