using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class StatementExpressionVariableDeclaratorList : QualifiedIdentifierStatementExpressionPrime
    {
        public List<VariableDeclarator> VariableDeclaratorList;
    }
}
