using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Utilities;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Ternary
{
    public class ConditionalOperator : TernaryOperator
    {
        public override string ToJS()
        {
            return $"({LeftOperand.ToJS()} ? {FirstRightOperand.ToJS()} : {SecondRightOperand.ToJS()})";
        }

        public override Type EvaluateType()
        {
            var condition = LeftOperand.EvaluateType();
            if(condition.ToString() != "bool")
                throw new SemanticException($"Condition in ternary operator must be of type bool at {Row} column {Col} in file {CompilerUtilities.FileName}.");

            var firstType = FirstRightOperand.EvaluateType();
            var secondType = SecondRightOperand.EvaluateType();
            if(firstType.ToString() != secondType.ToString())
                throw new SemanticException($"Both values before and after ':' in ternary operator must be of the same type at row {Row} column {Col} in file {CompilerUtilities.FileName}.");

            return firstType;
        }
    }
}
