using System;
using System.Collections.Generic;
using LexerAnalyser.Automata;
using LexerAnalyser.Enums;
using LexerAnalyser.Interfaces;
using LexerAnalyser.Models;

namespace LexerAnalyser
{
    public class Lexer
    {
        private readonly IInputStream _inputStream;
        private readonly Automaton _automaton;

        public Lexer(IInputStream inputStream)
        {
            _inputStream = inputStream;
            _automaton = new Automaton(inputStream);
        }

        public List<Token> GetTokens()
        {
            var tokens = new List<Token>();
            var token = _automaton.GetToken();

            while (token.Type != TokenType.Eof)
            {
                tokens.Add(token);
                token = _automaton.GetToken();
            }

            return tokens;
        }
    }
}