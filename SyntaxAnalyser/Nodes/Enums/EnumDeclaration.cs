using System.Collections.Generic;

namespace SyntaxAnalyser.Nodes.Enums
{
    public class EnumDeclaration : TypeDeclaration
    {
        public List<EnumMember> Members;
    }
}
