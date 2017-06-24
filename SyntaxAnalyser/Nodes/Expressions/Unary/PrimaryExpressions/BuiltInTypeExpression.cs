using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions
{
    public class BuiltInTypeExpression : Expression
    {
        public BuiltInDataType Type;

        public BuiltInTypeExpression(BuiltInDataType type)
        {
            Type = type;
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
