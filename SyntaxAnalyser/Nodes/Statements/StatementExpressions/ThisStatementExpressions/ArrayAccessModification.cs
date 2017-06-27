using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class ArrayAccessModification : QualifiedIdentifierStatementExpressionPrime
    {
        public List<Expression> ArrayAccessExpressionList;
        public ArrayAccess ArrayAccess;
        public ArrayAccessIncrementDecrementAssignment ArrayOperation;

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
