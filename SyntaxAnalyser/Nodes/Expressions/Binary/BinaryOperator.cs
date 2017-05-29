using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Binary
{
    public abstract class BinaryOperator : Expression
    {
        public Expression LeftOperand;
        public Expression RightOperand;
    }
}
