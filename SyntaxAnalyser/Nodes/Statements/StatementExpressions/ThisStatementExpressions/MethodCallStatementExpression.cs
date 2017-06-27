using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class MethodCallStatementExpression : QualifiedIdentifierStatementExpressionPrime
    {
        public List<Expression> ArgumentList;
        public CallAccess CallAccess;

        public MethodCallStatementExpression()
        {
            ArgumentList = new List<Expression>();
        }

        public override void ValidateSemantic()
        {
            //TODO
        }

        public override string GenerateJS()
        {
            //TODO
            return "";
        }
    }
}
