using System.Collections.Generic;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Statements.StatementExpressions;

namespace SyntaxAnalyser.Nodes.Statements.IterationStatements
{
    public class ForStatement : IterationStatement
    {
        public List<StatementExpression> Initializer;
        public Expression ConditionExpression;
        public List<StatementExpression> StatementExpressionList;
        public Statement StatementBody;
    }
}
