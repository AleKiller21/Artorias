﻿using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.TablesMetadata;
using SyntaxAnalyser.Utilities;

namespace SyntaxAnalyser.Nodes.Statements.JumpStatements
{
    public class BreakStatement : JumpStatement
    {
        public override void ValidateSemantic()
        {
            if (SymbolTable.GetInstance().CurrentScope.ScopeLabel != "whileStatement" &&
                SymbolTable.GetInstance().CurrentScope.ScopeLabel != "doStatement" &&
                SymbolTable.GetInstance().CurrentScope.ScopeLabel != "forStatement" &&
                SymbolTable.GetInstance().CurrentScope.ScopeLabel != "forEachStatement")
            {
                throw new SemanticException($"'break' statement is not valid in current context at row {Row} column {Col} in file {CompilerUtilities.FileName}.");
            }
        }

        public override string GenerateJS()
        {
            return "break;\n";
        }
    }
}
