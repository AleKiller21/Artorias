using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions
{
    public class BuiltInTypeExpression : Expression
    {
        public BuiltInDataType Type;

        public BuiltInTypeExpression(BuiltInDataType type)
        {
            Type = type;
        }
    }
}
