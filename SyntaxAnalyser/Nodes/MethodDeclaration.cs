using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes
{
    public abstract class MethodDeclaration : LineNumbering
    {
        public AccessModifier Modifier;
        public string Identifier;
        public DataType Type;
        public List<FixedParameter> Params;
    }
}
