using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.TablesMetadata;
using SyntaxAnalyser.Utilities;

namespace SyntaxAnalyser.Nodes.Statements.IterationStatements
{
    public class DoStatement : IterationStatement
    {
        public Statement StatementBody;
        public Expression ConditionExpression;

        public override void ValidateSemantic()
        {
            SymbolTable.GetInstance().PushScope(SymbolTable.GetInstance().CurrentScope.CurrentNamespace, CompilerUtilities.FileName, "doStatement");
            StatementBody.ValidateSemantic();
            CommonStatementValidations.ValidateConditionExpression(ConditionExpression, "do");
            SymbolTable.GetInstance().PopScope();
        }

        public override string GenerateJS()
        {
            return "";
        }
    }
}
