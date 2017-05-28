using System.Collections.Generic;

namespace SyntaxAnalyser.Nodes.Namespaces
{
    public class NamesapceDeclaration
    {
        public QualifiedIdentifier Identifier;
        public List<UsingNamespaceDeclaration> UsingNamespaces;
        public List<TypeDeclaration> Types;
        public List<NamesapceDeclaration> Declarations;

        public NamesapceDeclaration()
        {
            UsingNamespaces = new List<UsingNamespaceDeclaration>();
            Types = new List<TypeDeclaration>();
            Identifier = new QualifiedIdentifier();
            Declarations = new List<NamesapceDeclaration>();
        }
    }
}
