using System;
using LexerAnalyser;

namespace Artorias
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInputStream stream = new FileInputStream("C:\\Users\\alefe\\Documents\\Code\\C#\\Compiler\\Artorias\\Test\\sample.txt");
            Lexer lexer = new Lexer(stream);

            var tokens = lexer.GetTokens();
            foreach (var token in tokens)
            {
                Console.WriteLine(token.Lexeme + "-" + token.Row + "-" + token.Column + "-" + token.Type);
            }
            //Console.WriteLine("hola\0jack");
        }
    }
}