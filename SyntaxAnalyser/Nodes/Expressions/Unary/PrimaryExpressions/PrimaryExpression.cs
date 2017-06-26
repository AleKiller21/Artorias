using System.Collections.Generic;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions
{
    public class PrimaryExpression : Expression
    {
        public Expression PrimaryExpressionPrimePrime;
        public List<PrimaryExpressionPrime> PrimaryExpressionPrime;
        public override string ToJS()
        {
            //TODO Semantic: Por el momento solo funciona con literal. Arreglar.
            return PrimaryExpressionPrimePrime.ToJS();
        }

        public override Type EvaluateType()
        {
            //TODO Semantic: Revisar tambien el tipo de PrimaryExpressionPrime
            return PrimaryExpressionPrimePrime.EvaluateType();
        }
    }
}
