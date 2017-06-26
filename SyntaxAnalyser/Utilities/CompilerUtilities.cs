using System.Collections.Generic;
using SyntaxAnalyser.Nodes;
using SyntaxAnalyser.Nodes.Types;
using SyntaxAnalyser.TablesMetadata;

namespace SyntaxAnalyser.Utilities
{
    public class CompilerUtilities
    {
        public static string FileName;
        public static string GenerateMethodSignature(string identifier, List<FixedParameter> parameters)
        {
            var signature = identifier;
            foreach (var param in parameters)
            {
                signature += $",{param.Type.EvaluateType()}";
            }

            return signature;
        }

        public static string GetQualifiedName(QualifiedIdentifier identifier)
        {
            return string.Join(".", identifier.Identifiers.Identifiers);
        }

        public static Type GetTypeFromName(QualifiedIdentifier parent)
        {
            var parentName = CompilerUtilities.GetQualifiedName(parent);
            return SymbolTable.GetInstance().FindType(parentName);
        }
    }
}
