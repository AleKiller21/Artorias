using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Binary
{
    public class ConditionalAndOperator : BinaryOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} && {RightOperand.ToJS()})";
        }
    }
}
