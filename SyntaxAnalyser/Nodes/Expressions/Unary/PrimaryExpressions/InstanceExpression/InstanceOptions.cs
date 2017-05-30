using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions.InstanceExpression
{
    public class InstanceOptions
    {
        public List<Expression> ExpressionList;
        public List<Expression> ArgumentList;
        public List<DataType> Generic;

        public InstanceOptions()
        {
            ExpressionList = new List<Expression>();
            ArgumentList = new List<Expression>();
            Generic = new List<DataType>();
        }
    }
}
