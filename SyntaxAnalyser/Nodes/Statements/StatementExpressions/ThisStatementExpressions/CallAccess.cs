using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class CallAccess
    {
        public QualifiedIdentifier Identifier;
        public List<Expression> ArgumentList;
        public CallAccess Call;

        public CallAccess()
        {
            ArgumentList = new List<Expression>();
        }
    }
}
