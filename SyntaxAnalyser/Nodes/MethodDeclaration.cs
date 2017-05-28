﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes
{
    public class MethodDeclaration
    {
        public AccessModifier Modifier;
        public DataType Type;
        public string Identifier;
        public List<FixedParameter> Params;
    }
}
