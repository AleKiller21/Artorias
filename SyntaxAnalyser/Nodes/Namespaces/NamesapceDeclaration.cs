using System.Collections.Generic;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Namespaces
{
    public class NamesapceDeclaration : LineNumbering
    {
        public QualifiedIdentifier NamespaceIdentifier;
        public List<UsingNamespaceDeclaration> UsingDirectives;
        public List<TypeDeclaration> TypeDeclarations;
        public List<NamesapceDeclaration> NamespaceDeclarations;
        public string FileName;

        public NamesapceDeclaration(int row = 0, int col = 0)
        {
            UsingDirectives = new List<UsingNamespaceDeclaration>();
            TypeDeclarations = new List<TypeDeclaration>();
            NamespaceIdentifier = new QualifiedIdentifier();
            NamespaceDeclarations = new List<NamesapceDeclaration>();
            FileName = "";
            Row = row;
            Col = col;
        }
    }
}
