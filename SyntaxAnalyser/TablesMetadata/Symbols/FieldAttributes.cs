using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Classes;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.TablesMetadata.Symbols
{
    public class FieldAttributes : SymbolAttributes
    {
        private FieldDeclaration _field;

        public FieldAttributes(FieldDeclaration field, Type fieldType)
        {
            _field = field;
            SymbolType = fieldType;
        }
    }
}
