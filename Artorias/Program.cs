using System;
using System.Collections.Generic;

namespace Artorias
{
    class Program
    {
        static void Main(string[] args)
        {
            Compiler comp = new Compiler(args[0], args[1]);
            comp.Compile();
            Console.WriteLine("SUCCESS");

            //var json = JsonConvert.SerializeObject(code.GlobalNamespace, Formatting.Indented);
            //using (var sw = new StreamWriter(File.Create("C:\\Users\\alefe\\Documents\\Code\\C#\\Compiler\\GrammarCJ\\serialize.json")))
            //{
            //    sw.Write(json);
            //}
        }
    }
}