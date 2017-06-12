namespace SyntaxAnalyser.Nodes.Statements.SelectionStatements
{
    public class IfStatement : SelectionStatement
    {
        public Statement Statement;
        public ElseStatement ElseStatement;
    }
}
