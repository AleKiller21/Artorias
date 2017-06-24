using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Literal
{
    public class IntLiteral : LiteralExpression
    {
        public IntLiteral(int value, int row, int col)
        {
            Value = value;
            Row = row;
            Col = col;
        }

        public override Type EvaluateType()
        {
            return new IntType();
        }
    }
}
