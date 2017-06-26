using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Binary
{
    public class InclusiveOrOperator : BinaryOperator
    {
        public InclusiveOrOperator()
        {
            Rules["int,int"] = new IntType();
            Rules["int,char"] = new IntType();
            Rules["char,char"] = new IntType();
        }
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} | {RightOperand.ToJS()})";
        }
    }
}
