using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Binary
{
    public class AndOperator : BinaryOperator
    {
        public AndOperator()
        {
            Rules["int,int"] = new IntType();
            Rules["int,char"] = new IntType();
            Rules["char,char"] = new IntType();
        }
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} & {RightOperand.ToJS()})";
        }
    }
}
