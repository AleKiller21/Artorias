using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class ThisArrayRankSpecifierList : QualifiedIdentifierStatementExpressionPrime
    {
        public List<int> RankSpecifier;

        public ThisArrayRankSpecifierList()
        {
            RankSpecifier = new List<int>();
        }
    }
}
