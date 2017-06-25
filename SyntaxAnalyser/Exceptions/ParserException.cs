using System;
using SyntaxAnalyser.Utilities;

namespace SyntaxAnalyser.Exceptions
{
    public class ParserException : Exception
    {
        public ParserException(string message) : base($"{message} File: {CompilerUtilities.FileName}")
        {
        }
    }
}
