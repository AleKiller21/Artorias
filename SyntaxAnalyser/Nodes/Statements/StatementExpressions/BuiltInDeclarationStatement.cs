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

        public BuiltInDeclarationStatement(int row, int col)
        {
            OptionalRankSpecifierList = new List<int>();
            VariableDeclaratorList = new List<VariableDeclarator>();
            Row = row;
            Col = col;
        }

        public override void ValidateSemantic()
        {
            //TODO
        }

        public override string GenerateJS()
        {
            var declaratorsCode = "";
            foreach (var variableDeclarator in VariableDeclaratorList)
            {
                declaratorsCode +=
                    $"let {variableDeclarator.Identifier} = {variableDeclarator.VariableInitializer.Expression.ToJS()};\n";
            }

            return declaratorsCode;
        }
    }
}
