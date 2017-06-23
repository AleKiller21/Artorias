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

namespace Artorias
{
    public class Compiler
    {
        private readonly string _sourceDirectory;
        private string _destiny;

        public Compiler(string sourceDirectory, string destiny)
        {
            _sourceDirectory = sourceDirectory;
            _destiny = destiny;
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
            var fileCode = new List<Code>();
            foreach (var file in files)
            {
                var stream = new FileInputStream(file.FullName);
                var lexer = new Lexer(stream);
                var parser = new Parser(lexer);
                var code = parser.Parse();
                fileCode.Add(code);

                code.GlobalNamespace.FileName = file.Name;
                AddNamespaceEntry(code.GlobalNamespace, "", file.Name);
            }

            foreach (var code in fileCode)
            {
                //code.ValidateSemantic()
            }
        }

        private void AddDirectivesToUsingDirectivesTable(string namespaceName, string fileName, List<UsingNamespaceDeclaration> directives)
        {
            var directiveNames = directives.Select(directive => string.Join(".", directive.Identifier.Identifiers.Identifiers)).ToList();
            foreach (var directiveName in directiveNames)
            {
                if(!NamespaceTable.Namespaces.ContainsKey(directiveName))
                    throw new SemanticException($"The type or namespace '{directiveName}' could not be found.");
            }
            UsingDirectivesTable.Directives.Add($"{fileName},{namespaceName}", directiveNames);
        }

        private void AddNamespaceEntry(NamesapceDeclaration Namespace, string namespaceName, string fileName)
        {
            var entryName = namespaceName.Equals("") ? "global" : namespaceName;
            if (!NamespaceTable.Namespaces.ContainsKey(entryName))
            {
                NamespaceTable.Namespaces.Add(entryName, new Dictionary<string, Type>());
                AddDirectivesToUsingDirectivesTable(entryName, fileName, Namespace.UsingDirectives);
            }
            
            foreach (var typeDeclaration in Namespace.TypeDeclarations)
            {
                if(NamespaceTable.Namespaces[entryName].ContainsKey(typeDeclaration.Identifier))
                    throw new SemanticException($"<{entryName}> namespace already contains a definition for {typeDeclaration.Identifier}.");

                NamespaceTable.Namespaces[entryName].Add(typeDeclaration.Identifier, typeDeclaration);
            }

            foreach (var namespaceDeclaration in Namespace.NamespaceDeclarations)
            {
                if (entryName.Equals("global"))
                {
                    AddNamespaceEntry(namespaceDeclaration, string.Join(".", 
                        namespaceDeclaration.NamespaceIdentifier.Identifiers.Identifiers), fileName);
                }
                else
                {
                    AddNamespaceEntry(namespaceDeclaration, $"{entryName}.{string.Join(".", namespaceDeclaration.NamespaceIdentifier.Identifiers.Identifiers)}", fileName);
                }
            }
        }
    }
}
