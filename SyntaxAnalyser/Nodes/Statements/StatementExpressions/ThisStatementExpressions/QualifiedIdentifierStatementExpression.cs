using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class QualifiedIdentifierStatementExpression : StatementExpression
    {
        public QualifiedIdentifier Identifier;
        public QualifiedIdentifierStatementExpressionPrime ExpressionPrime;
    }
}
