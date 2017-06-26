using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Assignment
{
    public class AndEqualOperator : AssignmentOperator
    {
        public AndEqualOperator()
        {
            Rules["int,int"] = new IntType();
        }
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} &= {RightOperand.ToJS()})";
        }
    }
}
