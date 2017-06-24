using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions
{
    public abstract class Expression : LineNumbering
    {
        public abstract string ToJS();
        public abstract Type EvaluateType();
    }
}
