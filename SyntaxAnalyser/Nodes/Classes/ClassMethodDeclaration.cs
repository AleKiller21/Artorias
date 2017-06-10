using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Statements;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Classes
{
    public class ClassMethodDeclaration : ClassMemberDeclaration
    {
        public string Identifier;
        public List<FixedParameter> Params;
        public Statement Statements;
    }
}
