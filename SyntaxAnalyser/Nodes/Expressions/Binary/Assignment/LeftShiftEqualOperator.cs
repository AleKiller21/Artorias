using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Assignment
{
    public class LeftShiftEqualOperator : AssignmentOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} << {RightOperand.ToJS()})";
        }
    }
}
