using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions
{
    public class ParenthesizedStatementExpression : StatementExpression
    {
        public Expression ParenthesisExpression;
        public Expression OptionalExpression;
        public ArrayAccess ArrayAccess;
        public QualifiedIdentifier MemberAccessQualifiedIdentifier;
        public List<Expression> ArgumentList;
        public CallAccess CallAccess;
    }
}
