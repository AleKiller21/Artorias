using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Statements.ForEachStatement
{
    public class ForEachStatement : IterationStatement
    {
        public DataType IteratorType;
        public string IteratorIdentifier;
        public Expression EnumerableExpression;
        public Statement StatementBody;
    }
}
