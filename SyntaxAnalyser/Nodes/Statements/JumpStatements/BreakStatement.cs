using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Statements.JumpStatements
{
    public class BreakStatement : JumpStatement
    {
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
