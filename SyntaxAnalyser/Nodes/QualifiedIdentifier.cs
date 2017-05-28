using System.Collections.Generic;

namespace SyntaxAnalyser.Nodes
{
    public class QualifiedIdentifier
    {
        public List<string> Identifiers;

        public QualifiedIdentifier()
        {
            Identifiers = new List<string>();
        }
    }
}