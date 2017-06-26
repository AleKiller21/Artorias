using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Unary
{
    public class PreIncrementOperator : UnaryOperator
    {
        //TODO Semantic: Nunca se manda a llamar
        public PreIncrementOperator()
        {
            Rules["int"] = new IntType();
            Rules["float"] = new FloatType();
            Rules["char"] = new CharType();
        }
        public override string ToJS()
        {
            return $"(++{Operand.ToJS()})";
        }
    }
}
