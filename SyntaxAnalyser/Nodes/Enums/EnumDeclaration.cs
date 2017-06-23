using System.Collections.Generic;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Enums
{
    public class EnumDeclaration : TypeDeclaration
    {
        public List<EnumMember> Members;

        public override void ValidateSemantic()
        {
            throw new System.NotImplementedException();
        }

        public override string GenerateCode()
        {
            throw new System.NotImplementedException();
        }
    }
}
