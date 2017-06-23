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
        private static SymbolTable _instance;
        public readonly Dictionary<string, SymbolAttributes> Symbols;
        public string CurrentNamespace;
        public string FileName;
        public SymbolTable CurrentScope;
        private Stack<SymbolTable> _scope;

        private SymbolTable()
        {
            _scope = new Stack<SymbolTable>();
            Symbols = new Dictionary<string, SymbolAttributes>();
            //CurrentScope = this;
            //CurrentNamespace = namespaceName;
            //FileName = fileName;
        }

        public static SymbolTable GetInstance()
        {
            return _instance ?? (_instance = new SymbolTable());
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

        public void PushScope(string currentNamespace, string fileName)
        {
            _scope.Push(new SymbolTable());
            CurrentScope = _scope.Peek();
            CurrentScope.CurrentNamespace = currentNamespace;
            CurrentScope.FileName = fileName;
        }

        public void PopScope()
        {
            _scope.Pop();
            CurrentScope = _scope.Count == 0 ? null : _scope.Peek();
        }

        public void CheckSymbolDuplication(string identifier, SymbolAttributes attributes)
        {
            if (Symbols.ContainsKey(identifier))
                throw new SemanticException($"A symbol with the name {identifier} already exists within the current scope in file {FileName} at row {attributes.Row} column {attributes.Col}.");
        }
    }
}
