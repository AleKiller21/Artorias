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
            //XmlSerializer serializer = new XmlSerializer(typeof(NamesapceDeclaration));
            //serializer.Serialize(File.Create("C:\\Users\\alefe\\Documents\\Code\\C#\\Compiler\\serialize.xml"), code.GlobalNamespace);
            //var serializer = new JsonSerializer();
            var json = JsonConvert.SerializeObject(code.GlobalNamespace, Formatting.Indented);
            //using (var sw = new StreamWriter(File.Create("C:\\Users\\alefe\\Documents\\Code\\C#\\Compiler\\serialize.json")))
            //using (var writer = new JsonTextWriter(sw))
            //{
            //    serializer.Serialize(writer, code.GlobalNamespace);
            //}
            Console.WriteLine("SUCCESS");
            using (var sw = new StreamWriter(File.Create("C:\\Users\\alefe\\Documents\\Code\\C#\\Compiler\\serialize.json")))
            {
                sw.Write(json);
            }
        }
    }
}