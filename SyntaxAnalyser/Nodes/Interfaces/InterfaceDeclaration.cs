using System.Collections.Generic;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Interfaces
{
    public class InterfaceDeclaration : TypeDeclaration
    {
        public List<QualifiedIdentifier> Parents;
        public List<InterfaceMethodDeclaration> Methods;
    }
}
