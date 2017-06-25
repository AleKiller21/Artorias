using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Types
{
    public class BoolType : Type
    {
        public override string ToString()
        {
            return "bool";
        }

        public override string GetDefaultValue()
        {
            return "false";
        }
    }
}
