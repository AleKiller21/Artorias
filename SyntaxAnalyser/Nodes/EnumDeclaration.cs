using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes
{
    public class EnumDeclaration : TypeDeclaration
    {
        public List<EnumMember> Members;
    }
}
