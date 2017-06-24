using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Shift
{
    public class RightShiftOperator : ShiftOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} >> {RightOperand.ToJS()})";
        }
    }
}
