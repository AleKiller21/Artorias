using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.TablesMetadata;
using SyntaxAnalyser.Utilities;

namespace SyntaxAnalyser.Nodes.Statements.IterationStatements
{
    public class WhileStatement : IterationStatement
    {
        public Expression ConditionExpression;
        public Statement StatementBody;

        public override void ValidateSemantic()
        {
            SymbolTable.GetInstance().PushScope(SymbolTable.GetInstance().CurrentNamespace, CompilerUtilities.FileName, "whileStatement");
            CommonStatementValidations.ValidateConditionExpression(ConditionExpression, "while");
            StatementBody.ValidateSemantic();
            SymbolTable.GetInstance().PopScope();
        }

        public override string GenerateJS()
        {
            return "";
        }
    }
}
