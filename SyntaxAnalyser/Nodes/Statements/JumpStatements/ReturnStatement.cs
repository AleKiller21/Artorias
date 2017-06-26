using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Statements.JumpStatements
{
    public class ReturnStatement : JumpStatement
    {
        public Expression ReturnExpression;
        public override void ValidateSemantic()
        {
            throw new NotImplementedException();
        }

        public override string GenerateJS()
        {
            return $"return {ReturnExpression.ToJS()};\n";
        }
    }
}
