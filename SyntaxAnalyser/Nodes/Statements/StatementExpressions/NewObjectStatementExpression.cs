using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions
{
    class NewObjectStatementExpression : StatementExpression
    {
        public DataType Type;
        public List<Expression> ArgumentList;
        public CallAccess CallAccess;
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
