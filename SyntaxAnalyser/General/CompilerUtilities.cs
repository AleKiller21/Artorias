using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes;

namespace SyntaxAnalyser.General
{
    public class CompilerUtilities
    {
        public static string GenerateMethodSignature(string identifier, List<FixedParameter> parameters)
        {
            var signature = identifier;
            foreach (var param in parameters)
            {
                signature += $",{param.Type.EvaluateType()}";
            }

            return signature;
        }
    }
}
