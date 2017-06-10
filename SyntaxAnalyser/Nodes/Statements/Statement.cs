using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Statements
{
    public abstract class Statement
    {
        public List<Statement> StatementList;
    }
}
