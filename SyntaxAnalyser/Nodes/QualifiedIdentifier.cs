using System.Collections.Generic;

namespace SyntaxAnalyser.Nodes
{
    public class QualifiedIdentifier : LineNumbering
    {
        public IdentifierAttribute Identifiers;

        public QualifiedIdentifier()
        {
            Identifiers = new IdentifierAttribute(0, 0);
        }
    }
}