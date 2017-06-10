using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Statements.SwitchStatement
{
    public class SwitchSection
    {
        public List<SwitchLabel> Labels;
        public List<Statement> Statement;
    }
}
