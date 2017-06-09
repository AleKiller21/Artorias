using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions.InstanceExpression;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions
{
    public class PrimaryExpression : Expression
    {
        public Expression PrimaryExpressionPrimePrime;
        public List<PrimaryExpressionPrime> PrimaryExpressionPrime;
    }
}
