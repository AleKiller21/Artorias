using System.Collections.Generic;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Enums
{
    public class EnumDeclaration : TypeDeclaration
    {
        public List<EnumMember> Members;
    }
}
