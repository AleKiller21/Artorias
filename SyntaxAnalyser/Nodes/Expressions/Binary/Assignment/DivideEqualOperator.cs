using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Assignment
{
    public class DivideEqualOperator : AssignmentOperator
    {
        public DivideEqualOperator()
        {
            Rules["int,int"] = new IntType();
            Rules["float,float"] = new FloatType();
            Rules["float,char"] = new FloatType();
            Rules["float,int"] = new FloatType();
            Rules["int,char"] = new IntType();
            Rules["char,char"] = new IntType();
        }
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} /= {RightOperand.ToJS()})";
        }
    }
}
