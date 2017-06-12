using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Literal
{
    public class FloatLiteral : LiteralExpression
    {
        public FloatLiteral(float value, int row, int col)
        {
            Value = value;
            Row = row;
            Col = col;
        }
    }
}
