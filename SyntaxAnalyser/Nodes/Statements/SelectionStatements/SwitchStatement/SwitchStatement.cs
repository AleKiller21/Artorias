using System.Collections.Generic;

namespace SyntaxAnalyser.Nodes.Statements.SelectionStatements.SwitchStatement
{
    public class SwitchStatement : SelectionStatement
    {
        public List<SwitchSection> Sections;
        public override void ValidateSemantic()
        {
            throw new System.NotImplementedException();
        }

        public override string GenerateJS()
        {
            throw new System.NotImplementedException();
        }
    }
}
