using System.Collections.Generic;
using SyntaxAnalyser.Nodes;
using SyntaxAnalyser.Nodes.Classes;

namespace SyntaxAnalyser.TablesMetadata.Symbols
{
    public class MethodAttributes : SymbolAttributes
    {
        public AccessModifier Modifier;
        public OptionalModifier OptionalModifier;
        public string Identifier;
        public List<FixedParameter> Params;
    }
}
