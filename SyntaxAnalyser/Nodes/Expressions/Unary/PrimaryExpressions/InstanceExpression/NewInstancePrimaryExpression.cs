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

        public override string ToJS()
        {
            throw new System.NotImplementedException();
        }

        public override Type EvaluateType()
        {
            throw new System.NotImplementedException();
        }
    }
}
