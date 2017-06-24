using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.TablesMetadata.Symbols
{
    public abstract class SymbolAttributes : LineNumbering
    {
        protected Type SymbolType;
    }
}
