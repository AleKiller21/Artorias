using System.Collections.Generic;

namespace SyntaxAnalyser.Nodes.Classes
{
    class ClassDeclaration : TypeDeclaration
    {
        public bool IsAbstract;
        public List<QualifiedIdentifier> Parents;
    }
}
