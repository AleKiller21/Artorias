using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LexerAnalyser;
using SyntaxAnalyser.Nodes;
using SyntaxAnalyser.Parser;

namespace Artorias
{
    public class Compiler
    {
        private readonly string _sourceDirectory;
        private string _destiny;
        private readonly Dictionary<string, Code> _namespaceDictionary;

        public Compiler(string sourceDirectory, string destiny)
        {
            _sourceDirectory = sourceDirectory;
            _destiny = destiny;
            _namespaceDictionary = new Dictionary<string, Code>();
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
                var fileName = file.Name.Remove(file.Name.Length - 3);
                _namespaceDictionary.Add(currentNamespace + "." + fileName, code);
            }
        }
    }
}
