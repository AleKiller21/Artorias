using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Classes;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions.InstanceExpression
{
    public class NewInstanceOptionsCommaList : InstanceOptions2
    {
        public List<int> CommaList;
        public List<VariableInitializer> ArrayInitializer;

        public NewInstanceOptionsCommaList()
        {
            CommaList = new List<int>();
            ArrayInitializer = new List<VariableInitializer>();
            RankSpecifier = new List<int>();
        }
    }
}
