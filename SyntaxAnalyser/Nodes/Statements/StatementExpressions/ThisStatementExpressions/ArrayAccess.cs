using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class ArrayAccess
    {
        public List<Expression> ExpressionList;
        public ArrayAccess Access;

        public ArrayAccess()
        {
            ExpressionList = new List<Expression>();
        }
    }
}
