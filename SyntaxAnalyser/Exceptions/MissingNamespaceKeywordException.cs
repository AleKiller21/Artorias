using System;

namespace SyntaxAnalyser.Exceptions
{
    public class MissingNamespaceKeywordException : Exception
    {
        public MissingNamespaceKeywordException(string message) : base(message)
        {
        }
    }
}