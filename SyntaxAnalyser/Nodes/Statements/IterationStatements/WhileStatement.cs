using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Utilities;

namespace SyntaxAnalyser.Nodes.Statements.IterationStatements
{
    public class WhileStatement : IterationStatement
    {
        public Expression ConditionExpression;
        public Statement StatementBody;

        public override void ValidateSemantic()
        {
            CommonStatementValidations.ValidateConditionExpression(ConditionExpression, "while");
            StatementBody.ValidateSemantic();
        }

        public override string GenerateJS()
        {
            throw new System.NotImplementedException();
        }
    }
}
