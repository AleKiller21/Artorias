using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Classes
{
    public abstract class ClassMemberDeclaration : LineNumbering
    {
        public AccessModifier AccessModifier;
        public OptionalModifier OptionalModifier;
        public DataType Type;
    }
}
