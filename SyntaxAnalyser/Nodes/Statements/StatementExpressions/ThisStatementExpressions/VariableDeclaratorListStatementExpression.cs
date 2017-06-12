using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class VariableDeclaratorListStatementExpression : QualifiedIdentifierStatementExpressionPrime
    {
        public List<VariableDeclarator> VariableDeclaratorList;
    }
}
