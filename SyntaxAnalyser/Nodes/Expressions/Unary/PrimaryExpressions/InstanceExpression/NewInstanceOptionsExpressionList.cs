using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Classes;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions.InstanceExpression
{
    public class NewInstanceOptionsExpressionList : InstanceOptions2
    {
        public List<Expression> ExpressionList;
        public List<VariableInitializer> ArrayInitializer;

        public NewInstanceOptionsExpressionList()
        {
            ArrayInitializer = new List<VariableInitializer>();
            ExpressionList = new List<Expression>();
            RankSpecifier = new List<int>();
        }
    }
}
