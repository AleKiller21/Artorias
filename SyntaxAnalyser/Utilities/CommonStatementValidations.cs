using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Utilities
{
    public static class CommonStatementValidations
    {
        public static void ValidateConditionExpression(Expression ConditionExpression, string statementName)
        {
            if (ConditionExpression.EvaluateType().ToString() != "bool")
                throw new SemanticException($"Condition type in '{statementName}' statement must be of type bool at row {ConditionExpression.Row} column {ConditionExpression.Col} in file {CompilerUtilities.FileName}.");
        }
    }
}
