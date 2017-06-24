using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.IsAs
{
    public class AsOperator : IsAsOperator
    {
        //TODO Semantic: Find the right operator JS operator
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
