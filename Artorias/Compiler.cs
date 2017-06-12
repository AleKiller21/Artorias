using System;
using System.Collections.Generic;
using System.IO;
using LexerAnalyser;
using SemanticAnalyser;
using SemanticAnalyser.Exceptions;
using SyntaxAnalyser.Nodes;
using SyntaxAnalyser.Nodes.Namespaces;
using SyntaxAnalyser.Parser;

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
                ExploreDirectory(directory, project.Name + "." + directory.Name);
            }

            ParseFiles(project, project.Name);
            //var semantic = new Semantic();
            //foreach (var entry in NamespaceTable.Dictionary)
            //{
            //    semantic.Analyse(entry.Value);
            //}
        }

        private void ExploreDirectory(DirectoryInfo currentDirectory, string currentNamespace)
        {
            var directories = currentDirectory.GetDirectories();
            foreach (var directory in directories)
            {
                ExploreDirectory(directory, currentNamespace + "." + directory.Name);
            }

            ParseFiles(currentDirectory, currentNamespace);
        }

        private void ParseFiles(DirectoryInfo currentDirectory, string currentNamespace)
        {
            var files = currentDirectory.GetFiles();
            foreach (var file in files)
            {
                var stream = new FileInputStream(file.FullName);
                var lexer = new Lexer(stream);
                var parser = new Parser(lexer);
                var code = parser.Parse();

                //TODO Semantic: Tratar con los namespaces anidados dentro de otro namespace
                AddDictionaryEntry(code.GlobalNamespace, "");
            }
        }

        private void AddDictionaryEntry(NamesapceDeclaration Namespace, string namespaceName)
        {
            //foreach (var namespaceDeclaration in Namespace.NamespaceDeclarations)
            //{
            //    var name = namespaceName;
            //    foreach (var identifier in namespaceDeclaration.NamespaceIdentifier.Identifiers)
            //    {
            //        name += identifier + ".";
            //    }
            //    name = name.Remove(name.Length - 1);
            //    NamespaceTable.Dictionary.Add(name, namespaceDeclaration.TypeDeclarations);
            //    AddDictionaryEntry(namespaceDeclaration, name + ".");
            //}
        }
    }
}
