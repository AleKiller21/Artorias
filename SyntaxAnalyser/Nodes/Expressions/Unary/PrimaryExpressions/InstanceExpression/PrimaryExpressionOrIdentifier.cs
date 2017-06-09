using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions.InstanceExpression
{
    //TODO Borrar
    public abstract class PrimaryExpressionOrIdentifier : Expression
    {
        public List<PrimaryExpressionPrime> PrimeExpression;
    }
}
