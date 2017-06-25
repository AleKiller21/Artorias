using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Types
{
    public class StringType : Type
    {
        public override string ToString()
        {
            return "string";
        }

        public override string GetDefaultValue()
        {
            return "null";
        }
    }
}
