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
            //TODO Semantic: Analisar si utilizar el full qualified name en lugar de solo el nombre del tipo
            return Identifier;
        }

        public override string GetDefaultValue()
        {
            return "null";
        }
    }
}