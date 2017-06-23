using System;
using System.Collections.Generic;
using System.IO;
using LexerAnalyser;
using Newtonsoft.Json;
using SyntaxAnalyser.Parser;

namespace Artorias
{
    class Program
    {
        static void Main(string[] args)
        {
            Compiler comp = new Compiler(args[0], args[1]);
            comp.Compile();
            //var stream = new FileInputStream("C:\\Users\\alefe\\Desktop\\Daniel_expression.cs");
            //var lexer = new Lexer(stream);
            //var parser = new Parser(lexer);
            //var code = parser.Parse();
            //Console.WriteLine("SUCCESS");

            //var json = JsonConvert.SerializeObject(code.GlobalNamespace, Formatting.Indented);
            //using (var sw = new StreamWriter(File.Create("C:\\Users\\alefe\\Documents\\Code\\C#\\Compiler\\GrammarCJ\\serialize.json")))
            //{
            //    sw.Write(json);
            //}
        }
    }
}