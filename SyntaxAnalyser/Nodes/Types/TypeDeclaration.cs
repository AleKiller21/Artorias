namespace SyntaxAnalyser.Nodes.Types
{
    public abstract class TypeDeclaration : Type
    {
        public string Identifier;
        public AccessModifier Modifier;
        public abstract void ValidateSemantic();
        public abstract string GenerateCode();

        public override string ToString()
        {
            return Identifier;
        }

        public override string GetDefaultValue()
        {
            return "null";
        }
    }
}