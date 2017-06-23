using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes;

namespace SyntaxAnalyser.TablesMetadata.Symbols
{
    public abstract class SymbolAttributes : LineNumbering
    {
        protected Type SymbolType;
    }
}
