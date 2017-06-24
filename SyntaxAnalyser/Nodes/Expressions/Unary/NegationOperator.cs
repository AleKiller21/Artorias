using System;
using System.Collections.Generic;
using System.Text;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Unary
{
    public class NegationOperator : UnaryOperator
    {
        public override string ToJS()
        {
            return $"(!{Operand.ToJS()})";
        }

        public override Type EvaluateType()
        {
            //TODO Semantic: Implement Evaluate Type
            throw new NotImplementedException();
        }
    }
}
