using System;
using System.Collections.Generic;
using System.Text;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.TablesMetadata
{
    public class SymbolsTable
    {
        public Dictionary<string, Type> Vars;
        public string CurrentNamespace;
        public string FileName;
        public SymbolsTable Backward;
        public SymbolsTable Forward;

        public SymbolsTable(string namespaceName)
        {
            Vars = new Dictionary<string, Type>();
            CurrentNamespace = namespaceName;
        }
    }
}
