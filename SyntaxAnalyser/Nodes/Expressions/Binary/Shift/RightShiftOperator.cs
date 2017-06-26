using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Shift
{
    public class RightShiftOperator : ShiftOperator
    {
        public RightShiftOperator()
        {
            Rules["int,int"] = new IntType();
            Rules["int,char"] = new IntType();
            Rules["char,char"] = new IntType();
        }
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} >> {RightOperand.ToJS()})";
        }
    }
}
