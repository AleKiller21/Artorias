using System;
using System.Collections.Generic;
using System.Text;

namespace SemanticAnalyser.Exceptions
{
    public class FileRedeclaredWithinNamespaceException : Exception
    {
        public FileRedeclaredWithinNamespaceException(string file, string Namespace) : base($"File {file} has already been declared within {Namespace}.")
        {
            
        }
    }
}
