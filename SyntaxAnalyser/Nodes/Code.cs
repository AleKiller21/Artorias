using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Namespaces;

namespace SyntaxAnalyser.Nodes
{
    public class Code
    {
        public NamesapceDeclaration GlobalNamespace;

        public Code()
        {
            GlobalNamespace = new NamesapceDeclaration();
            GlobalNamespace.Identifier = new QualifiedIdentifier();
            GlobalNamespace.Identifier.Identifiers.Add("global");
        }
    }
}
