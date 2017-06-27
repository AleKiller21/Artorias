using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions.Unary;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class PostIncrementDecrementStatementExpression : QualifiedIdentifierStatementExpressionPrime
    {
        public UnaryOperator IncrementDecrement;
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
