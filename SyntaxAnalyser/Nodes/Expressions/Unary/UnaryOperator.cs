using System;
using System.Collections.Generic;
using System.Text;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Unary
{
    public abstract class UnaryOperator : Expression
    {
        public Expression Operand;

        public override Type EvaluateType()
        {
            //TODO Semantic: To implement
            throw new NotImplementedException();
        }
    }
}
