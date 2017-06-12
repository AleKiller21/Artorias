using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Statements
{
    public abstract class Statement : LineNumbering
    {
        public List<Statement> StatementList;
    }
}
