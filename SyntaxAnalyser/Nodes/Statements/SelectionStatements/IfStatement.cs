namespace SyntaxAnalyser.Nodes.Statements.SelectionStatements
{
    public class IfStatement : SelectionStatement
    {
        public Statement Statement;
        public ElseStatement ElseStatement;
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
