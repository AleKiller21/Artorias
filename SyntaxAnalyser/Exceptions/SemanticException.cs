using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Exceptions
{
    public class SemanticException : Exception
    {
        public SemanticException(string message) : base(message)
        {
            
        }
    }
}
