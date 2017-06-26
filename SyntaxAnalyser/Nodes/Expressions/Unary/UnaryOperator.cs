using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.TablesMetadata;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Expressions.Unary
{
    public abstract class UnaryOperator : Expression
    {
        public Expression Operand;
        public Dictionary<string, Type> Rules = new Dictionary<string, Type>();

        public override Type EvaluateType()
        {
            var rule = Operand.EvaluateType().ToString();
            if (Rules.ContainsKey(rule))
                return Rules[rule];

            throw new SemanticException($"Invalid operand type at row {Row} column {Col} in file {SymbolTable.GetInstance().CurrentScope.FileName}");
        }
    }
}
