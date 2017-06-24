using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Additive
{
    public class MinusOperator : AdditiveOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} - {RightOperand.ToJS()})";
        }
    }
}
