using System;
using System.Collections.Generic;
using System.Text;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Ternary
{
    public abstract class TernaryOperator : Expression
    {
        public Expression LeftOperand;
        public Expression FirstRightOperand;
        public Expression SecondRightOperand;
        
    }
}
