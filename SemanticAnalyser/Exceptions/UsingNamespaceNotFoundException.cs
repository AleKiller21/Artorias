using System;
using System.Collections.Generic;
using System.Text;

namespace SemanticAnalyser.Exceptions
{
    public class UsingNamespaceNotFoundException : Exception
    {
        public UsingNamespaceNotFoundException(string Namespace, int row, int col) : base($"The namespace {Namespace} at row {row} column {col} couldn't be found.")
        {
            
        }
    }
}
