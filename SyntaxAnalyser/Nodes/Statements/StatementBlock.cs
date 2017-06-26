using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.TablesMetadata;
using SyntaxAnalyser.Utilities;

namespace SyntaxAnalyser.Nodes.Statements
{
    public class StatementBlock : Statement
    {
        public StatementBlock()
        {
            StatementList = new List<Statement>();
        }

        public override void ValidateSemantic()
        {
            SymbolTable.GetInstance().PushScope(SymbolTable.GetInstance().CurrentScope.CurrentNamespace, CompilerUtilities.FileName);
            foreach (var statement in StatementList)
            {
                statement.ValidateSemantic();
            }
            SymbolTable.GetInstance().PopScope();
        }

        public override string GenerateJS()
        {
            throw new NotImplementedException();
        }
    }
}
