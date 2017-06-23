namespace SyntaxAnalyser.Nodes.Types
{
    public abstract class TypeDeclaration : Type
    {
        public string Identifier;
        public AccessModifier Modifier;
        public abstract void ValidateSemantic();
        public abstract string GenerateCode();
    }
}