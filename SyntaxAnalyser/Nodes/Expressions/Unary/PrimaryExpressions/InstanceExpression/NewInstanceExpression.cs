using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions.InstanceExpression
{
    public class NewInstanceExpression : Expression
    {
        public DataType InstanceType;
        public InstanceOptions Options;

        public NewInstanceExpression()
        {
            InstanceType = new DataType();
        }
    }
}
