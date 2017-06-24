using System.Collections.Generic;
using SyntaxAnalyser.Nodes.Types;

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

        public override string ToJS()
        {
            throw new System.NotImplementedException();
        }

        public override Type EvaluateType()
        {
            throw new System.NotImplementedException();
        }
    }
}
