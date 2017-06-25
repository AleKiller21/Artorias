using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Classes;

namespace SyntaxAnalyser.TablesMetadata.Symbols
{
    public class ConstructorAttributes : SymbolAttributes
    {
        private ConstructorDeclaration _constructor;

        public ConstructorAttributes(ConstructorDeclaration constructor)
        {
            _constructor = constructor;
            SymbolType = constructor.Type.EvaluateType();
        }
    }
}
