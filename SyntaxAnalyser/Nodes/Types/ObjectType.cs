using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Types
{
    public class ObjectType : Type
    {
        public override string ToString()
        {
            return "object";
        }

        public override string GetDefaultValue()
        {
            return "null";
        }
    }
}
