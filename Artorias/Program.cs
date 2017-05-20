using System;
using System.Text;
using LexerAnalyser;
using SyntaxAnalyser;

namespace Artorias
{
    class Program
    {
        static void Main(string[] args)
        {
            var stream = new FileInputStream(args[0]);
            var lexer = new Lexer(stream);
            var parser = new Parser(lexer);

            //var tokens = lexer.GetTokens();
            //foreach (var token in tokens)
            //{
            //    Console.WriteLine(token.Lexeme + " - " + "Row " + token.Row + " - " + "Column " + token.Column + " - " + "Type " + token.Type + "\n");
            //}
        }
    }
}