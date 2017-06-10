using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions.Unary;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class ThisStatementExpressionPostIncrementDecrement : QualifiedIdentifierStatementExpressionPrime
    {
        public UnaryOperator IncrementDecrement;
    }
}
