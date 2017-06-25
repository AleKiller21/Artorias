using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LexerAnalyser;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes;
using SyntaxAnalyser.Nodes.Namespaces;
using SyntaxAnalyser.Parser;
using SyntaxAnalyser.TablesMetadata;
using SyntaxAnalyser.Utilities;
using Type = SyntaxAnalyser.Nodes.Types.Type;

//TODO Agregar 'Dictionary' como keyword. Validarlo en la produccion NonArrayType
namespace Artorias
{
    public class Compiler
    {
        private readonly string _sourceDirectory;
        private string _destiny;
        private string _fileName;
        private string _codeOutput;
        private readonly List<Code> _codeFiles;
        private SymbolTable _symbolTable;

        public Compiler(string sourceDirectory, string destiny)
        {
            _sourceDirectory = sourceDirectory;
            _destiny = destiny;
            _codeFiles = new List<Code>();
        }

        public void Compile()
        {
            var project = new DirectoryInfo(_sourceDirectory);
            var directories = project.GetDirectories();

            foreach (var directory in directories)
            {
                ExploreDirectory(directory);
            }

            CompileFiles(project);
            var x = NamespaceTable.Namespaces;
            var y = UsingDirectiveTable.Directives;
        }

        private void ExploreDirectory(DirectoryInfo currentDirectory)
        {
            var directories = currentDirectory.GetDirectories();
            foreach (var directory in directories)
            {
                ExploreDirectory(directory);
            }

            CompileFiles(currentDirectory);
        }

        private void CompileFiles(DirectoryInfo currentDirectory)
        {
            ParseFiles(currentDirectory);
            FillNamespaceTable();
            FillUsingDirectiveTable();
            GenerateCode();
        }

        private void ParseFiles(DirectoryInfo currentDirectory)
        {
            var files = currentDirectory.GetFiles();
            foreach (var file in files)
            {
                CompilerUtilities.FileName = file.Name;
                var stream = new FileInputStream(file.FullName);
                var lexer = new Lexer(stream);
                var parser = new Parser(lexer);
                var code = parser.Parse();

                code.GlobalNamespace.FileName = file.Name;
                _codeFiles.Add(code);
            }
        }

        private void FillNamespaceTable()
        {
            foreach (var code in _codeFiles)
            {
                _fileName = code.GlobalNamespace.FileName;
                AddNamespaceEntry(code.GlobalNamespace, "");
            }
        }

        private void FillUsingDirectiveTable()
        {
            foreach (var code in _codeFiles)
            {
                _fileName = code.GlobalNamespace.FileName;
                AddUsingDirectives(code.GlobalNamespace, "");
            }
        }

        private void GenerateCode()
        {
            foreach (var code in _codeFiles)
            {
                _fileName = code.GlobalNamespace.FileName;
                SymbolTable.GetInstance().PushScope("global", _fileName);
                GenerateJS(code.GlobalNamespace);
                SymbolTable.GetInstance().PopScope();
            }
        }

        private void GenerateJS(NamesapceDeclaration Namespace)
        {
            var entryName = SymbolTable.GetInstance().CurrentScope.CurrentNamespace.Equals("global") ? 
                "global" : SymbolTable.GetInstance().CurrentScope.CurrentNamespace;

            foreach (var typeDeclaration in Namespace.TypeDeclarations)
            {
                SymbolTable.GetInstance().PushScope(entryName, _fileName);
                _codeOutput += typeDeclaration.GenerateCode();
                SymbolTable.GetInstance().PopScope();
            }

            foreach (var namespaceDeclaration in Namespace.NamespaceDeclarations)
            {
                if (entryName.Equals("global"))
                {
                    SymbolTable.GetInstance().PushScope(string.Join(".",
                        namespaceDeclaration.NamespaceIdentifier.Identifiers.Identifiers), _fileName);

                    GenerateJS(namespaceDeclaration);
                    SymbolTable.GetInstance().PopScope();
                }

                else
                {
                    SymbolTable.GetInstance().PushScope($"{entryName}.{string.Join(".", namespaceDeclaration.NamespaceIdentifier.Identifiers.Identifiers)}", _fileName);

                    GenerateJS(namespaceDeclaration);
                    SymbolTable.GetInstance().PopScope();
                }
            }
        }

        private void AddNamespaceEntry(NamesapceDeclaration Namespace, string namespaceName)
        {
            var entryName = namespaceName.Equals("") ? "global" : namespaceName;
            if (!NamespaceTable.Namespaces.ContainsKey(entryName))
                NamespaceTable.Namespaces.Add(entryName, new Dictionary<string, Type>());

            foreach (var typeDeclaration in Namespace.TypeDeclarations)
            {
                if(NamespaceTable.Namespaces[entryName].ContainsKey(typeDeclaration.Identifier))
                    throw new SemanticException($"<{entryName}> namespace already contains a definition for {typeDeclaration.Identifier}.");

                NamespaceTable.Namespaces[entryName].Add(typeDeclaration.Identifier, typeDeclaration);
            }

            foreach (var namespaceDeclaration in Namespace.NamespaceDeclarations)
            {
                namespaceDeclaration.FileName = _fileName;
                if (entryName.Equals("global"))
                {
                    AddEntryForEveryNamespaceIdentifier(
                        namespaceDeclaration.NamespaceIdentifier.Identifiers.Identifiers, entryName);

                    AddNamespaceEntry(namespaceDeclaration, string.Join(".", 
                        namespaceDeclaration.NamespaceIdentifier.Identifiers.Identifiers));
                }
                else
                {
                    AddEntryForEveryNamespaceIdentifier(
                        namespaceDeclaration.NamespaceIdentifier.Identifiers.Identifiers, entryName);

                    AddNamespaceEntry(namespaceDeclaration, $"{entryName}.{string.Join(".", namespaceDeclaration.NamespaceIdentifier.Identifiers.Identifiers)}");
                }
            }
        }

        private void AddEntryForEveryNamespaceIdentifier(List<string> identifiers, string currentNamespaceName)
        {
            var namespaceName = currentNamespaceName.Equals("global") ? "" : $"{currentNamespaceName}.";
            for (var i = 0; i < identifiers.Count - 1; i++)
            {
                namespaceName += identifiers[i];
                var namespaceDeclaration = new NamesapceDeclaration();
                AddNamespaceEntry(namespaceDeclaration, namespaceName);
                namespaceName += ".";
            }
        }

        private void AddUsingDirectives(NamesapceDeclaration Namespace, string namespaceName)
        {
            var entryName = namespaceName.Equals("") ? "global" : namespaceName;
            AddDirectivesToUsingDirectivesTable(entryName, Namespace.UsingDirectives);

            foreach (var namespaceDeclaration in Namespace.NamespaceDeclarations)
            {
                if (entryName.Equals("global"))
                {
                    AddUsingDirectives(namespaceDeclaration, string.Join(".",
                        namespaceDeclaration.NamespaceIdentifier.Identifiers.Identifiers));
                }
                else
                {
                    AddUsingDirectives(namespaceDeclaration, $"{entryName}.{string.Join(".", namespaceDeclaration.NamespaceIdentifier.Identifiers.Identifiers)}");
                }
            }
        }

        private void AddDirectivesToUsingDirectivesTable(string namespaceName, List<UsingNamespaceDeclaration> directives)
        {
            var directiveNames = new List<string>();
            foreach (var directive in directives)
            {
                var directiveName = string.Join(".", directive.Identifier.Identifiers.Identifiers);
                if (!NamespaceTable.Namespaces.ContainsKey(directiveName))
                    throw new SemanticException($"The type or namespace '{directiveName}' at row {directive.Row} column {directive.Col} in file {_fileName} could not be found.");

                directiveNames.Add(directiveName);
            }

            if(UsingDirectiveTable.Directives.ContainsKey($"{_fileName},{namespaceName}"))
                UsingDirectiveTable.Directives[$"{_fileName},{namespaceName}"].AddRange(directiveNames);

            else
            {
                UsingDirectiveTable.Directives.Add($"{_fileName},{namespaceName}", directiveNames);
            }
        }
    }
}
