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
using Type = SyntaxAnalyser.Nodes.Types.Type;

//TODO Agregar 'Dictionary' como keyword. Validarlo en la produccion NonArrayType
namespace Artorias
{
    public class Compiler
    {
        private readonly string _sourceDirectory;
        private string _destiny;
        private string _fileName;
        private List<Code> _codeFiles;

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
            var y = UsingDirectivesTable.Directives;
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
            var files = currentDirectory.GetFiles();
            foreach (var file in files)
            {
                var stream = new FileInputStream(file.FullName);
                var lexer = new Lexer(stream);
                var parser = new Parser(lexer);
                var code = parser.Parse();
                _codeFiles.Add(code);

                code.GlobalNamespace.FileName = file.Name;
                _fileName = file.Name;
                AddNamespaceEntry(code.GlobalNamespace, "");
            }

            foreach (var code in _codeFiles)
            {
                _fileName = code.GlobalNamespace.FileName;
                AddUsingDirectives(code.GlobalNamespace, "");
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

            if(UsingDirectivesTable.Directives.ContainsKey($"{_fileName},{namespaceName}"))
                UsingDirectivesTable.Directives[$"{_fileName},{namespaceName}"].AddRange(directiveNames);

            else
            {
                UsingDirectivesTable.Directives.Add($"{_fileName},{namespaceName}", directiveNames);
            }
        }
    }
}
