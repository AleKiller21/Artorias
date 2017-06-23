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

        public NamesapceDeclaration(int row, int col)
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
