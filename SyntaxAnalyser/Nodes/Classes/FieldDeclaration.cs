using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Classes
{
    public class FieldDeclaration : ClassMemberDeclaration
    {
        //TODO Semantic: Solo los fields arreglos pueden usar array initializers.
        public string Identifier;
        public VariableInitializer Value;
        public List<FieldDeclaration> Declarators;

        public FieldDeclaration()
        {
            Declarators = new List<FieldDeclaration>();
        }
    }
}
