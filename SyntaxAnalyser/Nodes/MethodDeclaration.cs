using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes
{
    public abstract class MethodDeclaration
    {
        public AccessModifier Modifier;
        public DataType Type;
        public string Identifier;
        public List<FixedParameter> Params;
    }
}
