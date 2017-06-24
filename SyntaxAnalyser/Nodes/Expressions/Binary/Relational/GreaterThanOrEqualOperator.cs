﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Relational
{
    public class GreaterThanOrEqualOperator : RelationalOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} >= {RightOperand.ToJS()})";
        }
    }
}
