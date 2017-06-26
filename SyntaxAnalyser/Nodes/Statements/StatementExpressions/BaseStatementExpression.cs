using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions
{
    public class BaseStatementExpression : StatementExpression
    {
        public string MethodIdentifier;
        public List<Expression> ArgumentList;
        public CallAccess CallAccess;
        public override void ValidateSemantic()
        {
            throw new NotImplementedException();
        }

        public override string GenerateJS()
        {
            throw new NotImplementedException();
        }
    }
}
