using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Types
{
    public abstract class Type : LineNumbering
    {
        public abstract string GetDefaultValue();
    }
}
