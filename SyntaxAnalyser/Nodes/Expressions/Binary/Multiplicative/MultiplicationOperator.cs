using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Multiplicative
{
    public class MultiplicationOperator : MultiplicativeOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} * {RightOperand.ToJS()})";
        }
    }
}
