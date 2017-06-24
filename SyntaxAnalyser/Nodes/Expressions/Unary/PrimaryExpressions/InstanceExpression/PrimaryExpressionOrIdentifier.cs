using System;
using System.Collections.Generic;
using System.Text;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions.InstanceExpression
{
    //TODO Borrar
    public abstract class PrimaryExpressionOrIdentifier : Expression
    {
        public List<PrimaryExpressionPrime> PrimeExpression;
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
