﻿using System.Collections.Generic;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Statements.StatementExpressions;
using SyntaxAnalyser.TablesMetadata;
using SyntaxAnalyser.Utilities;

namespace SyntaxAnalyser.Nodes.Statements.IterationStatements
{
    public class ForStatement : IterationStatement
    {
        public List<StatementExpression> Initializer;
        public Expression ConditionExpression;
        public List<StatementExpression> StatementExpressionList;
        public Statement StatementBody;

        public override void ValidateSemantic()
        {
            SymbolTable.GetInstance().PushScope(SymbolTable.GetInstance().CurrentNamespace, CompilerUtilities.FileName, "forStatement");
            foreach (var statementExpression in Initializer)
            {
                //statementExpression.ValidateSemantic();
            }
            CommonStatementValidations.ValidateConditionExpression(ConditionExpression, "for");
            foreach (var statementExpression in StatementExpressionList)
            {
                //statementExpression.ValidateSemantic();
            }
            StatementBody.ValidateSemantic();
            SymbolTable.GetInstance().PopScope();
        }

        public override string GenerateJS()
        {
            throw new System.NotImplementedException();
        }
    }
}
