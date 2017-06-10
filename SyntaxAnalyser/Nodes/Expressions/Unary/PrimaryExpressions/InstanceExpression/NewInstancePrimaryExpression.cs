using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions.InstanceExpression
{
    public class NewInstancePrimaryExpression : Expression
    {
        public DataType InstanceType;
        public InstanceOptions Options;

        public NewInstancePrimaryExpression()
        {
            InstanceType = new DataType();
        }
    }
}
