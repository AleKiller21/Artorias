using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions.Binary.Equality;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Binary
{
    public class ConditionalAndOperator : EqualityOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} && {RightOperand.ToJS()})";
        }
    }
}
