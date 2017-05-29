using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Literal
{
    public class StringLiteral : LiteralExpression
    {
        public StringLiteral(string value)
        {
            Value = value;
        }
    }
}
