namespace SyntaxAnalyser.Nodes.Expressions.Binary.IsAs
{
    public class IsOperator : IsAsOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} instanceof {RightOperand.ToJS()})";
        }
    }
}
