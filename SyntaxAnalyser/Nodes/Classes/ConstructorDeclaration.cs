using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Statements;

namespace SyntaxAnalyser.Nodes.Classes
{
    public class ConstructorDeclaration : ClassMemberDeclaration
    {
        public bool IsStatic;
        public List<FixedParameter> Params;
        public List<Expression> ParentConstructorArguments;
        public Statement Statements;

        public ConstructorDeclaration()
        {
            Params = new List<FixedParameter>();
            ParentConstructorArguments = new List<Expression>();
        }
    }
}
