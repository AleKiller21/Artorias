using SyntaxAnalyser.Utilities;

namespace SyntaxAnalyser.Nodes.Statements.SelectionStatements
{
    public class IfStatement : SelectionStatement
    {
        public Statement Statement;
        public ElseStatement ElseStatement;
        public override void ValidateSemantic()
        {
            CommonStatementValidations.ValidateConditionExpression(TestValue, "if");
            Statement.ValidateSemantic();
            if(this.ElseStatement != null)
                this.ElseStatement.Statement.ValidateSemantic();
        }

        public override string GenerateJS()
        {
            return "";
        }
    }
}
