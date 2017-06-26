using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Unary
{
    public class BitwiseNegationOperator : UnaryOperator
    {
        public BitwiseNegationOperator()
        {
            Rules["int"] = new IntType();
            Rules["char"] = new IntType();
        }
        public override string ToJS()
        {
            return $"(~{Operand.ToJS()})";
        }
    }
}
