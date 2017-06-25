using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.TablesMetadata;

namespace SyntaxAnalyser.Exceptions
{
    public class GeneralSemanticException : SemanticException
    {
        public GeneralSemanticException(int row, int col, string fileName, string typeName) : base($"The type or namespace '{typeName}' at row {row} column {col} in file {fileName} could not be found.")
        {
        }
    }
}
