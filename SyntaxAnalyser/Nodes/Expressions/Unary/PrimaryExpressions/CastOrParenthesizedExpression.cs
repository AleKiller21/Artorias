using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions
{
    public class CastOrParenthesizedExpression : Expression
    {
        public Expression ParethesisExpression;
        public Expression NonParenthesisExpression;
    }
}
