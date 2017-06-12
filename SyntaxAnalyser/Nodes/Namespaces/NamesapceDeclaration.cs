using System.Collections.Generic;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Namespaces
{
    public class NamesapceDeclaration : LineNumbering
    {
        public QualifiedIdentifier NamespaceIdentifier;
        public List<UsingNamespaceDeclaration> UsingNamespaces;
        public List<TypeDeclaration> TypeDeclarations;
        public List<NamesapceDeclaration> NamespaceDeclarations;

        public NamesapceDeclaration(int row, int col)
        {
            UsingNamespaces = new List<UsingNamespaceDeclaration>();
            TypeDeclarations = new List<TypeDeclaration>();
            NamespaceIdentifier = new QualifiedIdentifier();
            NamespaceDeclarations = new List<NamesapceDeclaration>();
            Row = row;
            Col = col;
        }
    }
}
