using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Unary
{
    public abstract class UnaryOperator : Expression
    {
        public Expression Operand;
    }
}
