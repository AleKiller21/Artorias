using System;
using LexerAnalyser;
using LexerAnalyser.Enums;
using LexerAnalyser.Models;

namespace SyntaxAnalyser
{
    public class Parser
    {
        private Lexer _lexer;
        private Token _token;

        public Parser(Lexer lexer)
        {
            _lexer = lexer;
            _token = _lexer.GetToken();
        }

        public void Parse()
        {
            
        }

        private void Code()
        {
            throw new NotImplementedException();
        }
    }
}
