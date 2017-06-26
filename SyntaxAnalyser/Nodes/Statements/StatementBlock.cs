using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Statements
{
    public class StatementBlock : Statement
    {
        public StatementBlock()
        {
            StatementList = new List<Statement>();
        }

        public override void ValidateSemantic()
        {
            foreach (var statement in StatementList)
            {
                statement.ValidateSemantic();
            }
        }

        public override string GenerateJS()
        {
            throw new NotImplementedException();
        }
    }
}
