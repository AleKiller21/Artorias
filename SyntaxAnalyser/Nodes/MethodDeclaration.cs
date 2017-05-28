using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes
{
    public class MethodDeclaration
    {
        public AccessModifier modifier;
        public DataType Type;
        public string Identifier;
        public List<FixedParameter> Params;
    }
}
