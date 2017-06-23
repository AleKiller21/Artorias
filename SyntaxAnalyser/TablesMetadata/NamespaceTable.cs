using System;
using System.Collections.Generic;
using System.Text;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.TablesMetadata
{
    public class NamespaceTable
    {
        public static Dictionary<string, Dictionary<string, Type> > Namespaces = new Dictionary<string, Dictionary<string, Type> >();
    }
}
