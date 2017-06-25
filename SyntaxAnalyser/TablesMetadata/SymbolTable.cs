using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.TablesMetadata.Symbols;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.TablesMetadata
{
    //TODO Semantic: Validar cuando los tipos esten usando el full qualified name
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

        public SymbolAttributes LookForSymbonInParents()
        {
            throw new NotImplementedException();
        }

        public Type FindType(string identifier)
        {
            //TODO Semantic: Fix using directives ambiguous reference
            var currentNamespace = _scope.Peek().CurrentNamespace;
            var type = CheckForTypeInCurrentNamespace(identifier, currentNamespace);
            if (type != null) return type;

            type = CheckInUsingDirectives(identifier, currentNamespace);
            if (type != null) return type;

            foreach (var table in _scope)
            {
                if(table.CurrentNamespace.Equals(currentNamespace)) continue;

                currentNamespace = table.CurrentNamespace;
                type = CheckForTypeInCurrentNamespace(identifier, currentNamespace);
                if (type != null) return type;

                type = CheckInUsingDirectives(identifier, currentNamespace);
                if (type != null) return type;
            }

            return null;
        }

        private Type CheckForTypeInCurrentNamespace(string identifier, string Namespace)
        {
            return NamespaceTable.Namespaces[Namespace].ContainsKey(identifier) ? 
                NamespaceTable.Namespaces[Namespace][identifier] : null;
        }

        private Type CheckInUsingDirectives(string identifier, string Namespace)
        {
            var directives = UsingDirectiveTable.Directives[$"{SymbolTable.GetInstance().CurrentScope.FileName},{Namespace}"];
            foreach (var directive in directives)
            {
                var type = CheckForTypeInCurrentNamespace(identifier, directive);
                if(type == null) continue;
                return type;
            }

            return null;
        }
        public string FindTypeNamespace(string typeIdentifier)
        {
            //TODO Semantic: Fix using directives ambiguous reference
            var currentNamespace = _scope.Peek().CurrentNamespace;
            var typeNamespace = TryGetNamespace(typeIdentifier, currentNamespace);
            if (typeNamespace != "") return typeNamespace;

            typeNamespace = GetNamespaceTypeThroughDirectives(typeIdentifier, currentNamespace);
            if (typeNamespace != "") return typeNamespace;

            foreach (var table in _scope)
            {
                if (table.CurrentNamespace.Equals(currentNamespace)) continue;

                currentNamespace = table.CurrentNamespace;
                typeNamespace = TryGetNamespace(typeIdentifier, currentNamespace);
                if (typeNamespace != "") return typeNamespace;

                typeNamespace = GetNamespaceTypeThroughDirectives(typeIdentifier, currentNamespace);
                if (typeNamespace != "") return typeNamespace;
            }

            return "";
        }

        private string TryGetNamespace(string identifier, string Namespace)
        {
            return NamespaceTable.Namespaces[Namespace].ContainsKey(identifier) ? Namespace : "";
        }

        private string GetNamespaceTypeThroughDirectives(string identifier, string Namespace)
        {
            var directives = UsingDirectiveTable.Directives[$"{SymbolTable.GetInstance().CurrentScope.FileName},{Namespace}"];
            foreach (var directive in directives)
            {
                var namespaceType = TryGetNamespace(identifier, directive);
                if (namespaceType == "") continue;
                return namespaceType;
            }

            return "";
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

        public void CheckSymbolDuplication(string identifier, int row, int col, string errMessage="")
        {
            if (Symbols.ContainsKey(identifier))
            {
                if(errMessage == "")
                    throw new SemanticException($"A symbol with the name {identifier} already exists within the current scope in file {FileName} at row {row} column {col}.");

                throw new SemanticException($"{errMessage} '{identifier}' at row {row} column {col} in {FileName}.");
            }
        }
    }
}
