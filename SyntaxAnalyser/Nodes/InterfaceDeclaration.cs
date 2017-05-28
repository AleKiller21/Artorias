using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes
{
    public class InterfaceDeclaration : TypeDeclaration
    {
        public List<QualifiedIdentifier> Parents;
        public List<MethodDeclaration> Methods;
    }
}
