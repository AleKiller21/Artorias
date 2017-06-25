using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Types
{
    public class IntType : Type
    {
        public override string ToString()
        {
            return "int";
        }

        public override string GetDefaultValue()
        {
            return "0";
        }
    }
}
