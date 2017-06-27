using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions
{
    public abstract class QualifiedIdentifierStatementExpressionPrime : LineNumbering
    {
        public abstract void ValidateSemantic();
        public abstract string GenerateJS();
    }
}
