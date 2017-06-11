using System.Collections.Generic;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Classes
{
    class ClassDeclaration : TypeDeclaration
    {
        public bool IsAbstract;
        public List<QualifiedIdentifier> Parents;
        public List<ClassMemberDeclaration> Members;
    }
}
