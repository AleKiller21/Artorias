namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions
{
    public abstract class StatementExpression : Statement
    {
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
