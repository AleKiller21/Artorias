using SyntaxAnalyser.Nodes.Namespaces;

namespace SyntaxAnalyser.Nodes
{
    public class Code
    {
        public NamesapceDeclaration GlobalNamespace;

        public Code()
        {
            GlobalNamespace = new NamesapceDeclaration(0, 0);
            GlobalNamespace.NamespaceIdentifier = new QualifiedIdentifier();
            GlobalNamespace.NamespaceIdentifier.Identifiers.Identifiers.Add("global");
        }
    }
}
