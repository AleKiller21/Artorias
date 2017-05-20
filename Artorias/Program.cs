using System;
using System.Text;
using LexerAnalyser;
using SyntaxAnalyser;
using SyntaxAnalyser.Parser;

namespace Artorias
{
    class Program
    {
        static void Main(string[] args)
        {
            var stream = new FileInputStream(args[0]);
            var lexer = new Lexer(stream);
            var parser = new Parser(lexer);
            parser.Parse();
            Console.WriteLine("SUCCESS");
        }
    }
}