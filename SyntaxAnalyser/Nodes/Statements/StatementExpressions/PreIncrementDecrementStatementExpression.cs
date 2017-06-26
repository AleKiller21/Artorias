using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions.Unary;
using SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions
{
    class PreIncrementDecrementStatementExpression : StatementExpression
    {
        public bool UsingThisKeyword;
        public UnaryOperator IncrementDecrement;
        public QualifiedIdentifier Identifier;
        public ArrayAccess ArrayAccess;
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
