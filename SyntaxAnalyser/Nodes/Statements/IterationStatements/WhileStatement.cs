using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Statements.IterationStatements
{
    public class WhileStatement : IterationStatement
    {
        public Expression ConditionExpression;
        public Statement StatementBody;
    }
}
