using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions.Binary.Equality;

namespace SyntaxAnalyser.Nodes.Expressions.Binary
{
    public class ConditionalOrOperator : EqualityOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} || {RightOperand.ToJS()})";
        }
    }
}
