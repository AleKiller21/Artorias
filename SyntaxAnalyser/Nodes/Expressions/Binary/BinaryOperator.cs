using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.TablesMetadata;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Binary
{
    public abstract class BinaryOperator : Expression
    {
        public Expression LeftOperand;
        public Expression RightOperand;
        public Dictionary<string, Type> Rules = new Dictionary<string, Type>();

        public override Type EvaluateType()
        {
            var leftType = LeftOperand.EvaluateType().ToString();
            var rightType = RightOperand.EvaluateType().ToString();
            var rule = $"{leftType},{rightType}";

            if (Rules.ContainsKey(rule))
                return Rules[rule];

            throw new SemanticException($"Invalid operand types at row {Row} column {Col} in file {SymbolTable.GetInstance().CurrentScope.FileName}");
        }
    }
}
