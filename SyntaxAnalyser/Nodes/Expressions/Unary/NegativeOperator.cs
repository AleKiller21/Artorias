using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Unary
{
    public class NegativeOperator : UnaryOperator
    {
        public NegativeOperator()
        {
            Rules["char"] = new IntType();
            Rules["int"] = new IntType();
            Rules["float"] = new FloatType();
        }
        public override string ToJS()
        {
            return $"(-{Operand.ToJS()})";
        }
    }
}
