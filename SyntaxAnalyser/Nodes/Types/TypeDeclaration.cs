namespace SyntaxAnalyser.Nodes.Types
{
    public abstract class TypeDeclaration : LineNumbering
    {
        public string Identifier;
        public AccessModifier Modifier;
    }
}