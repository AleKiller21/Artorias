using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Statements.JumpStatements
{
    public class ReturnStatement : JumpStatement
    {
        public Expression ReturnExpression;
    }
}
