using System;
using System.IO;
using System.Text;
using LexerAnalyser;
using SyntaxAnalyser;
using SyntaxAnalyser.Parser;
using System.Xml.Serialization;
using SyntaxAnalyser.Nodes;

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
            XmlSerializer serializer = new XmlSerializer(typeof(NamesapceDeclaration));
            serializer.Serialize(File.Create("C:\\Users\\alefe\\Documents\\Code\\C#\\Compiler\\serialize.xml"), code.GlobalNamespace);
            Console.WriteLine("SUCCESS");
        }
    }
}