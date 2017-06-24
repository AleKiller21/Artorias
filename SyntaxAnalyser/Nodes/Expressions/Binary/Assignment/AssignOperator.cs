using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Assignment
{
    public class AssignOperator : AssignmentOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} = {RightOperand.ToJS()})";
        }
    }
}
