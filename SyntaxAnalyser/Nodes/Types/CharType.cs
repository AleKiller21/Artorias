using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Types
{
    public class CharType : Type
    {
        public override string ToString()
        {
            return "char";
        }

        public override string GetDefaultValue()
        {
            return "0";
        }
    }
}
