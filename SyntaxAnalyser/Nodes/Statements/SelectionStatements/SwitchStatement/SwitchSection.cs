using System.Collections.Generic;
using SyntaxAnalyser.Nodes.Statements.SwitchStatement;

namespace SyntaxAnalyser.Nodes.Statements.SelectionStatements.SwitchStatement
{
    public class SwitchSection : LineNumbering
    {
        public List<SwitchLabel> Labels;
        public List<Statement> Statement;
    }
}
