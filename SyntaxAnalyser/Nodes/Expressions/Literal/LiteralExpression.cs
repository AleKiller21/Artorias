using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Literal
{
    public abstract class LiteralExpression : Expression
    {
        public dynamic Value;
        public override string ToJS()
        {
            return Value.ToString();
        }
    }
}
