using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Assignment
{
    public class LeftShiftEqualOperator : AssignmentOperator
    {
        public LeftShiftEqualOperator()
        {
            Rules["int,int"] = new IntType();
        }
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} << {RightOperand.ToJS()})";
        }
    }
}
