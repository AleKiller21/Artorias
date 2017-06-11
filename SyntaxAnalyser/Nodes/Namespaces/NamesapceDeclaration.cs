using System.Collections.Generic;

namespace SyntaxAnalyser.Nodes.Namespaces
{
    public class NamesapceDeclaration
    {
        public QualifiedIdentifier NamespaceIdentifier;
        public List<UsingNamespaceDeclaration> UsingNamespaces;
        public List<TypeDeclaration> TypeDeclarations;
        public List<NamesapceDeclaration> NamespaceDeclarations;

        public NamesapceDeclaration()
        {
            UsingNamespaces = new List<UsingNamespaceDeclaration>();
            TypeDeclarations = new List<TypeDeclaration>();
            NamespaceIdentifier = new QualifiedIdentifier();
            NamespaceDeclarations = new List<NamesapceDeclaration>();
        }
    }
}
