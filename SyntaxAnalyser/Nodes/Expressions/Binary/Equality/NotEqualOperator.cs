using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Equality
{
    public class NotEqualOperator : EqualityOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} !== {RightOperand.ToJS()})";
        }
    }
}
