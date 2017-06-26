using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Unary
{
    public class NegationOperator : UnaryOperator
    {
        public NegationOperator()
        {
            Rules["bool"] = new BoolType();
        }
        public override string ToJS()
        {
            return $"(!{Operand.ToJS()})";
        }
    }
}
