using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Expressions.Binary.Assignment;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class StatementExpressionAssignment : QualifiedIdentifierStatementExpressionPrime
    {
        public AssignmentOperator Operator;
        public Expression ExpressionValue;
    }
}
