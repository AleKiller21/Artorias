using System.Collections.Generic;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Interfaces
{
    public class InterfaceDeclaration : TypeDeclaration
    {
        public List<QualifiedIdentifier> Parents;
        public List<InterfaceMethodDeclaration> Methods;

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
