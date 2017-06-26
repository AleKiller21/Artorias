using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Multiplicative
{
    public class ModuloOperator : MultiplicativeOperator
    {
        public ModuloOperator()
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
        }
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} % {RightOperand.ToJS()})";
        }
    }
}
