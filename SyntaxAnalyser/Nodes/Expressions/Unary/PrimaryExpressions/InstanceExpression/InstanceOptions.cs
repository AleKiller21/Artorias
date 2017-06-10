using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions.InstanceExpression
{
    public class InstanceOptions
    {
        //TODO Semantic: Si options no es null, entonces ignorar los demas campos. Solo revisar options
        public InstanceOptions2 options;
        public List<Expression> ArgumentList;
        public List<DataType> Generic;

        public InstanceOptions()
        {
            ArgumentList = new List<Expression>();
            Generic = new List<DataType>();
        }
    }
}
