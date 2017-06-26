using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Additive
{
    public class SumOperator : AdditiveOperator
    {
        public SumOperator()
        {
            Rules["int,int"] = new IntType();
            Rules["int,char"] = new IntType();
            Rules["char,int"] = new IntType();
            Rules["char,char"] = new IntType();

            Rules["int,float"] = new FloatType();
            Rules["float,float"] = new FloatType();
            Rules["float,int"] = new FloatType();
            Rules["float,char"] = new FloatType();
            Rules["char,float"] = new IntType();

            Rules["string,char"] = new StringType();
            Rules["string,string"] = new StringType();
            Rules["string,int"] = new StringType();
            Rules["string,float"] = new StringType();
            Rules["char,string"] = new StringType();
            Rules["int,string"] = new StringType();
        }
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} + {RightOperand.ToJS()})";
        }
    }
}
