using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions
{
    public class BuiltInDeclarationStatement : StatementExpression
    {
        public bool IsVar;
        public BuiltInDataType BuiltInDataType;
        public List<int> OptionalRankSpecifierList;
        public List<VariableDeclarator> VariableDeclaratorList;

        public BuiltInDeclarationStatement()
        {
            OptionalRankSpecifierList = new List<int>();
            VariableDeclaratorList = new List<VariableDeclarator>();
        }
    }
}
