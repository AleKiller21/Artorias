using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Statements
{
    public class StatementBlock : Statement
    {
        public StatementBlock()
        {
            StatementList = new List<Statement>();
        }
    }
}
