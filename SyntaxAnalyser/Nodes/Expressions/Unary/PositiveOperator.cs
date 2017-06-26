using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Unary
{
    public class PositiveOperator : UnaryOperator
    {
        public PositiveOperator()
        {
            Rules["int"] = new IntType();
            Rules["char"] = new IntType();
            Rules["float"] = new FloatType();
        }
        public override string ToJS()
        {
            return $"(+{Operand.ToJS()})";
        }
    }
}
