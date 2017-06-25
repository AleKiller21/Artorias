using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Utilities;

namespace SyntaxAnalyser.Exceptions
{
    public class SemanticException : Exception
    {
        public SemanticException(string message) : base(message)
        {
            
        }
    }
}
