using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions.InstanceExpression;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions
{
    public class PrimaryExpressionIdentifier : PrimaryExpressionOrIdentifier
    {
        public string Identifier;

        public PrimaryExpressionIdentifier(string identifier)
        {
            Identifier = identifier;
        }
    }
}
