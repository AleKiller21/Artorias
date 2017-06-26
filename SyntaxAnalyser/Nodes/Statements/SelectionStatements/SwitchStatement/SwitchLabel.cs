using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Statements.SelectionStatements.SwitchStatement
{
    public class SwitchLabel : LineNumbering
    {
        public Label Label;
        public Expression Expression; //TODO Semantic: Solo cuando el label es 'case'
    }
}
