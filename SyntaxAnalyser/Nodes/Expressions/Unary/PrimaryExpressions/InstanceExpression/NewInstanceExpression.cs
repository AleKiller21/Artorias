using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions.InstanceExpression
{
    public class NewInstanceExpression : PrimaryExpression
    {
        public DataType InstanceType;
        public InstanceOptions Options;

        public NewInstanceExpression()
        {
            InstanceType = new DataType();
        }
    }
}
