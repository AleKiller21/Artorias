using System;
using System.Collections.Generic;
using System.Text;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.TablesMetadata.Symbols
{
    public class ClassAttribute : SymbolAttributes
    {
        public ClassAttribute(Type classDeclaration)
        {
            SymbolType = classDeclaration;
        }
    }
}
