using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Interfaces;
using LexerAnalyser.Models;

namespace LexerAnalyser
{
    public class Automaton
    {
        private IInputStream _inputStream;

        public Automaton(IInputStream inputStream)
        {
            _inputStream = inputStream;
        }

        public Token GetToken()
        {
            var symbol = _inputStream.GetNextSymbol();
            while (symbol.Character != '$')
            {
                Console.WriteLine(symbol.Character + " " + symbol.RowCount + " " + symbol.ColCount);
                symbol = _inputStream.GetNextSymbol();
            }

            return null;
        }
    }
}
