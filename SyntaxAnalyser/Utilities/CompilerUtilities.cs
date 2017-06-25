using System.Collections.Generic;
using SyntaxAnalyser.Nodes;

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
    }
}
