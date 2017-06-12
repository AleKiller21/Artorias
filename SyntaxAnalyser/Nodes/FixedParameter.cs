﻿using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes
{
    public class FixedParameter : LineNumbering
    {
        public DataType Type;
        public string Identifier;
    }
}
