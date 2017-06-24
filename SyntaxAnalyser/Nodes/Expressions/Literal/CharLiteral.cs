using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Literal
{
    public class CharLiteral : LiteralExpression
    {
        public CharLiteral(char value, int row, int col)
        {
            Value = value;
            Row = row;
            Col = col;
        }

        public override Type EvaluateType()
        {
            return new CharType();
        }
    }
}
