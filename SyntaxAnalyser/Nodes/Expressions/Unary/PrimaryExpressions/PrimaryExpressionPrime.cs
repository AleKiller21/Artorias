using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions
{
    public class PrimaryExpressionPrime : Expression
    {
        public List<Expression> ArgumentList; //(argument-list)
        public List<Expression> ExpressionList; //[expression-list]
        public QualifiedIdentifier IdentifierAttribute;
        public UnaryOperator Operator; //increment-decrement

        public PrimaryExpressionPrime()
        {
            ArgumentList = new List<Expression>();
            ExpressionList = new List<Expression>();
        }
    }
}
