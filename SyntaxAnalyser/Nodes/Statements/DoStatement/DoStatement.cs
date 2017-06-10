using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Statements.DoStatement
{
    public class DoStatement : IterationStatement
    {
        public Statement StatementBody;
        public Expression ConditionExpression;
    }
}
