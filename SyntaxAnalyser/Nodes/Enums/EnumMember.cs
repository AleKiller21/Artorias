using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Enums
{
    public class EnumMember : LineNumbering
    {
        public string Identifier;
        public Expression Value;
    }
}
