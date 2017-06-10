using System.Collections.Generic;

namespace SyntaxAnalyser.Nodes.Statements.SwitchStatement
{
    public class SwitchStatement : SelectionStatement
    {
        public List<SwitchSection> Sections;
    }
}
