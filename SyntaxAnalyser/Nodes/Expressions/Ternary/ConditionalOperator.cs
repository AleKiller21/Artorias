using System;
using System.Collections.Generic;
using System.Text;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Ternary
{
    public class ConditionalOperator : TernaryOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} ? {FirstRightOperand.ToJS()} : {SecondRightOperand.ToJS()})";
        }

        public override Type EvaluateType()
        {
            //TODO Semantic: EvaluateType
            throw new NotImplementedException();
        }
    }
}
