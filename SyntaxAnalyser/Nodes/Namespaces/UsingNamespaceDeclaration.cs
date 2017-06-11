namespace SyntaxAnalyser.Nodes.Namespaces
{
    public class UsingNamespaceDeclaration : LineNumbering
    {
        public QualifiedIdentifier Identifier;

        public UsingNamespaceDeclaration(QualifiedIdentifier identifier)
        {
            Identifier = identifier;
        }

        public UsingNamespaceDeclaration()
        {
            
        }
    }
}