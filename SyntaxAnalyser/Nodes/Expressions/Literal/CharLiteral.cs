using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Literal
{
    public class CharLiteral : LiteralExpression
    {
        public CharLiteral(char value)
        {
            Value = value;
        }
    }
}
