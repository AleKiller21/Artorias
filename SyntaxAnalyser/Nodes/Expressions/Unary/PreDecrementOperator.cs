using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Unary
{
    public class PreDecrementOperator : UnaryOperator
    {
        public PreDecrementOperator()
        {
            Rules["int"] = new IntType();
            Rules["float"] = new FloatType();
            Rules["char"] = new CharType();
        }
        public override string ToJS()
        {
            return $"(--{Operand.ToJS()})";
        }

        public override Type EvaluateType()
        {
            throw new NotImplementedException();
        }
    }
}
