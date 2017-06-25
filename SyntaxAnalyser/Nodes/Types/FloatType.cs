using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Types
{
    public class FloatType : Type
    {
        public override string ToString()
        {
            return "float";
        }

        public override string GetDefaultValue()
        {
            return "0";
        }
    }
}
