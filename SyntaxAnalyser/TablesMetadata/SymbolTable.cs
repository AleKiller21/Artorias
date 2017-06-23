using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.TablesMetadata.Symbols;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.TablesMetadata
{
    public class SymbolTable
    {
        public readonly Dictionary<string, SymbolAttributes> Symbols;
        public readonly string CurrentNamespace;
        public readonly string FileName;
        public SymbolTable CurrentScope;
        private readonly Stack<SymbolTable> _scope;

        public SymbolTable(string namespaceName, string fileName)
        {
            _scope = new Stack<SymbolTable>();
            Symbols = new Dictionary<string, SymbolAttributes>();
            CurrentScope = this;
            CurrentNamespace = namespaceName;
            FileName = fileName;
        }

        public void InsertSymbol(string identifier, SymbolAttributes attributes)
        {
            Symbols.Add(identifier, attributes);
        }

        public SymbolAttributes GetSymbol(string identifier)
        {
            foreach (var table in _scope)
            {
                if (table.Symbols.ContainsKey(identifier))
                    return table.Symbols[identifier];
            }

            return null;
        }

        public void CreateNewScope()
        {
            var table = new SymbolTable(CurrentNamespace, FileName);
            _scope.Push(table);
            CurrentScope = table;
        }

        public void ExitScope()
        {
            _scope.Pop();
            CurrentScope = _scope.Peek();
        }

        public void CheckSymbolDuplication(string identifier, SymbolAttributes attributes)
        {
            if (Symbols.ContainsKey(identifier))
                throw new SemanticException($"A symbol with the name {identifier} already exists within the current scope in file {FileName} at row {attributes.Row} column {attributes.Col}.");
        }
    }
}
