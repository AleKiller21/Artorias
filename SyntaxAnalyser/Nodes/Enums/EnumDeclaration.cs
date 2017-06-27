using System;
using System.Collections.Generic;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Types;
using SyntaxAnalyser.TablesMetadata;
using SyntaxAnalyser.TablesMetadata.Symbols;
using SyntaxAnalyser.Utilities;

namespace SyntaxAnalyser.Nodes.Enums
{
    public class EnumDeclaration : TypeDeclaration
    {
        public List<EnumMember> Members;

        public override void ValidateSemantic()
        {
            if (Modifier != AccessModifier.Public && Modifier != AccessModifier.None)
                throw new SemanticException($"{Identifier} access modifier is invalid at row {Row} column {Col} in file {SymbolTable.GetInstance().CurrentScope.FileName}.");

            foreach (var member in Members)
            {
                SymbolTable.GetInstance()
                    .CurrentScope.CheckSymbolDuplication(member.Identifier, member.Row, member.Col);

                var typeName = member.Value.EvaluateType().ToString();
                if (!typeName.Equals("int") && !typeName.Equals("char"))
                    throw new SemanticException($"Enum member {member.Identifier} at file {SymbolTable.GetInstance().CurrentScope.FileName} row {member.Row} column {member.Col} must be of type int or char.");


                SymbolTable.GetInstance().CurrentScope.InsertSymbol(member.Identifier, new EnumMemberAttribute(member.Value, this));
            }
        }

        public override string GenerateCode()
        {
            ValidateSemantic();
            var code = $"class {CompilerUtilities.GetFullQualifiedTypeName(Identifier)}";
            code += " { constructor() {";

            foreach (var member in Members)
            {
                code += $"this.{member.Identifier} = {member.Value.ToJS()}; ";
            }
            code += "} }";

            return code;
        }
    }
}
