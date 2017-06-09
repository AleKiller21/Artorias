using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
