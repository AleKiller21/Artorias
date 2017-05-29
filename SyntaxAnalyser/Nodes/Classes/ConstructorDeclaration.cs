using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Statements;

namespace SyntaxAnalyser.Nodes.Classes
{
    public class ConstructorDeclaration : ClassMemberDeclaration
    {
        public bool IsStatic;
        public List<FixedParameter> Params;
        public List<Statement> Statements;
    }
}
