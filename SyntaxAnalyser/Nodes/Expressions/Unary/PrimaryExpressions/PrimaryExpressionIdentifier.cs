using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions.InstanceExpression;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions
{
    public class PrimaryExpressionIdentifier : PrimaryExpressionOrIdentifier
    {
        public string Identifier;

        public PrimaryExpressionIdentifier(string identifier)
        {
            Identifier = identifier;
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
