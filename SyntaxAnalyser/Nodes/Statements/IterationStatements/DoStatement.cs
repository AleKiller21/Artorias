using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Utilities;

namespace SyntaxAnalyser.Nodes.Statements.IterationStatements
{
    public class DoStatement : IterationStatement
    {
        public Statement StatementBody;
        public Expression ConditionExpression;

        public override void ValidateSemantic()
        {
            StatementBody.ValidateSemantic();
            CommonStatementValidations.ValidateConditionExpression(ConditionExpression, "do");
        }

        public override string GenerateJS()
        {
            throw new System.NotImplementedException();
        }
    }
}
