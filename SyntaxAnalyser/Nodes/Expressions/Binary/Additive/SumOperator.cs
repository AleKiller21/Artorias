using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Additive
{
    public class SumOperator : AdditiveOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} + {RightOperand.ToJS()})";
        }
    }
}
