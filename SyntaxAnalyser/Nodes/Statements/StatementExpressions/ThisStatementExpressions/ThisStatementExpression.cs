namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class ThisStatementExpression : StatementExpression
    {
        public QualifiedIdentifierStatementExpression StatementExpression;
        public override void ValidateSemantic()
        {
            throw new System.NotImplementedException();
        }

        public override string GenerateJS()
        {
            throw new System.NotImplementedException();
        }
    }
}
