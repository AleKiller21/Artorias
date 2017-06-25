using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Classes;

namespace SyntaxAnalyser.TablesMetadata.Symbols
{
    public class FieldAttributes : SymbolAttributes
    {
        private FieldDeclaration _field;

        public FieldAttributes(FieldDeclaration field)
        {
            _field = field;
        }
    }
}
