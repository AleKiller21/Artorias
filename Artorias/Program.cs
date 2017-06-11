using System;
using System.IO;
using LexerAnalyser;
using SyntaxAnalyser.Parser;
using Newtonsoft.Json;

namespace Artorias
{
    class Program
    {
        static void Main(string[] args)
        {
            var stream = new FileInputStream(args[0]);
            var lexer = new Lexer(stream);
            var parser = new Parser(lexer);
            var code = parser.Parse();
            //var json = JsonConvert.SerializeObject(code.GlobalNamespace, Formatting.Indented);
            Console.WriteLine("SUCCESS");
            //using (var sw = new StreamWriter(File.Create("C:\\Users\\alefe\\Documents\\Code\\C#\\Compiler\\GrammarCJ\\serialize.json")))
            //{
            //    sw.Write(json);
            //}
        }
    }
}