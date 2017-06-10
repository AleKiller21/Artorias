﻿using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Classes
{
    public class VariableInitializer
    {
        public Expression Expression;
        public List<VariableInitializer> ArrayInitializers;

        public VariableInitializer()
        {
            ArrayInitializers = new List<VariableInitializer>();
        }
    }
}
