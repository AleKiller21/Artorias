using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class ArrayAccess : LineNumbering
    {
        public List<Expression> ExpressionList;
        public ArrayAccess Access;

        public ArrayAccess(int row, int col)
        {
            ExpressionList = new List<Expression>();
            Row = row;
            Col = col;
        }
    }
}
