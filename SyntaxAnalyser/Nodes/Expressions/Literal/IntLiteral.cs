using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Literal
{
    public class IntLiteral : LiteralExpression
    {
        public IntLiteral(int value)
        {
            Value = value;
        }
    }
}
