using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Statements
{
    public abstract class SelectionStatement : Statement
    {
        public Expression TestValue;
    }
}
