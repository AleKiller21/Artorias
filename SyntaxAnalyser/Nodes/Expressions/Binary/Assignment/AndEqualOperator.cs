using System;
using System.Collections.Generic;
using System.Text;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Assignment
{
    public class AndEqualOperator : AssignmentOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} &= {RightOperand.ToJS()})";
        }
    }
}
