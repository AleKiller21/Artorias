using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Statements.SwitchStatement;

namespace SyntaxAnalyser.Nodes.Statements.SelectionStatements.SwitchStatement
{
    public class SwitchLabel : LineNumbering
    {
        public Label Label;
        public Expression Expression; //TODO Semantic: Solo cuando el label es 'case'
    }
}
