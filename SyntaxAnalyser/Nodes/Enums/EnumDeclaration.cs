using System;
using System.Collections.Generic;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Types;
using SyntaxAnalyser.TablesMetadata;

namespace SyntaxAnalyser.Nodes.Enums
{
    public class EnumDeclaration : TypeDeclaration
    {
        public List<EnumMember> Members;

        public override void ValidateSemantic()
        {
            foreach (var member in Members)
            {
                SymbolTable.GetInstance()
                    .CurrentScope.CheckSymbolDuplication(member.Identifier, member.Row, member.Col);

                //TODO Semantic: Implement EvaluateType() for all Expression Nodes.
                //var typeName = member.Value.EvaluateType().toString();
                //if(!typeName.Equals("int") && !typeName.Equals("char"))
                //    throw new SemanticException($"Enum member {member.Identifier} at file {SymbolTable.GetInstance().CurrentScope.FileName} row {member.Row} column {member.Col} must be of type int or char.");
            }
        }

        public override string GenerateCode()
        {
            ValidateSemantic();
            var code = $"class {SymbolTable.GetInstance().CurrentScope.CurrentNamespace.Replace(".", string.Empty)}{Identifier}";
            code += " { constructor() {";

            foreach (var member in Members)
            {
                //code += $"this.{member.Identifier} = {member.Value.ToJS()}; ";
            }
            code += "} }";

            return code;
        }
    }
}
