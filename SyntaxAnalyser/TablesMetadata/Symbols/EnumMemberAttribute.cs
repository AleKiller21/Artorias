using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.TablesMetadata.Symbols
{
    public class EnumMemberAttribute : SymbolAttributes
    {
        public Expression ExpressionValue { get; }

        public EnumMemberAttribute(Expression value, Type memberType)
        {
            ExpressionValue = value;
            SymbolType = memberType;
        }
    }
}
