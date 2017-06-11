using System;
using System.Linq;
using SemanticAnalyser.Exceptions;
using SyntaxAnalyser.Nodes;

namespace SemanticAnalyser
{
    public class Semantic
    {
        public void Analyse(Code code)
        {
            var usingNamespaces = code.GlobalNamespace.UsingNamespaces;
            var exists = true;
            foreach (var usingNamespace in usingNamespaces)
            {
                var usingNamespaceName = "";
                foreach (var identifier in usingNamespace.Identifier.Identifiers)
                {
                    usingNamespaceName += identifier + ".";
                }
                usingNamespaceName = usingNamespaceName.Remove(usingNamespaceName.Length - 1);

                foreach(var entry in NamespaceTable.Dictionary)
                {
                    var entryNamespace = entry.Key.Remove(entry.Key.LastIndexOf('.'));
                    if (entryNamespace.Equals(usingNamespaceName))
                    {
                        exists = true;
                        break;
                    }
                    exists = false;
                }
                if(!exists) throw new UsingNamespaceNotFoundException(usingNamespaceName, usingNamespace.Row, usingNamespace.Col);
            }
        }
    }
}
