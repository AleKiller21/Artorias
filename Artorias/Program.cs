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

            lexer.GetTokens();
        }
    }
}