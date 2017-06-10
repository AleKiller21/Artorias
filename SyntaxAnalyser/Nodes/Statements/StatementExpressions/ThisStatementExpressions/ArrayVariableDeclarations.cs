using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class ArrayVariableDeclarations : QualifiedIdentifierStatementExpressionPrime
    {
        public List<int> RankSpecifier;
        public List<VariableDeclarator> Declarators;

        public ArrayVariableDeclarations()
        {
            RankSpecifier = new List<int>();
            Declarators = new List<VariableDeclarator>();
        }
    }
}
