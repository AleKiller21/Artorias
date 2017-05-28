namespace SyntaxAnalyser.Nodes
{
    public class UsingNamespaceDeclaration
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