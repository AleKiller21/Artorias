using System.Collections.Generic;

namespace SyntaxAnalyser.Nodes.Statements.SelectionStatements.SwitchStatement
{
    public class SwitchStatement : SelectionStatement
    {
        public List<SwitchSection> Sections;
    }
}
