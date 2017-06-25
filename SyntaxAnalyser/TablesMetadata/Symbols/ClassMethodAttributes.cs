using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Classes;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.TablesMetadata.Symbols
{
    public class ClassMethodAttributes : SymbolAttributes
    {
        private ClassMethodDeclaration _method;

        public ClassMethodAttributes(ClassMethodDeclaration method, Type methodType)
        {
            _method = method;
            SymbolType = methodType;
        }
    }
}
