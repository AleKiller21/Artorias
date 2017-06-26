using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Types
{
    public class NullType : Type
    {
        public override string GetDefaultValue()
        {
            return "null";
        }
    }
}
