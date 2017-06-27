using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Utilities;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public class QualifiedIdentifierStatementExpression : StatementExpression
    {
        public QualifiedIdentifier Identifier;
        public QualifiedIdentifierStatementExpressionPrime ExpressionPrime;
        public override void ValidateSemantic()
        {
            
        }

        public override string GenerateJS()
        {
            return $"{CompilerUtilities.GetQualifiedName(Identifier)} = {ExpressionPrime.GenerateJS()};";
        }
    }
}
