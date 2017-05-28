using System.Collections.Generic;

namespace SyntaxAnalyser.Nodes.Interfaces
{
    public class InterfaceDeclaration : TypeDeclaration
    {
        public List<QualifiedIdentifier> Parents;
        public List<InterfaceMethodDeclaration> Methods;
    }
}
