using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Statements.WhileStatement
{
    public class WhileStatement : IterationStatement
    {
        public Expression ConditionExpression;
        public Statement StatementBody;
    }
}
