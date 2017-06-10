using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Statements.IterationStatements
{
    public class DoStatement : IterationStatement
    {
        public Statement StatementBody;
        public Expression ConditionExpression;
    }
}
