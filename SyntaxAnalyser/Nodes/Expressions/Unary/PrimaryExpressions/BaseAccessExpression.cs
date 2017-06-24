using System;
using System.Collections.Generic;
using System.Text;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions
{
    public class BaseAccessExpression : Expression
    {
        public string Identifier;
        public List<Expression> ExpressionList;

        public BaseAccessExpression()
        {
            ExpressionList = new List<Expression>();
        }

        public override string ToJS()
        {
            throw new NotImplementedException();
        }

        public override Type EvaluateType()
        {
            throw new NotImplementedException();
        }
    }
}
