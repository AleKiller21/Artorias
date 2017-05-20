using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser
{
    public class ParserException : Exception
    {
        public ParserException(string message) : base(message)
        {
        }
    }
}
