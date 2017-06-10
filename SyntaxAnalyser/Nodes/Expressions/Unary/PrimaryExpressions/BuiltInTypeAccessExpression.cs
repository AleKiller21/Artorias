using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions
{
    public class BuiltInTypeAccessExpression : Expression
    {
        public BuiltInDataType BuiltInType;
        //TODO Semantic: validar si identifier es null o no. Si no es null, entonces revisar el chain call
        public string Identifier;
        public CallAccess CallAccess;
    }
}
