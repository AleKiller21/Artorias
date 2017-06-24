using System;
using System.Collections.Generic;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.General;
using SyntaxAnalyser.Nodes.Types;
using SyntaxAnalyser.TablesMetadata;
using Type = SyntaxAnalyser.Nodes.Types.Type;

namespace SyntaxAnalyser.Nodes.Interfaces
{
    public class InterfaceDeclaration : TypeDeclaration
    {
        public List<QualifiedIdentifier> Parents;
        public List<InterfaceMethodDeclaration> Methods;

        public override void ValidateSemantic()
        {
            var parentsDictionary = new Dictionary<string, Type>();
            var methodsDictionary = new Dictionary<string, string>();
            var currentFile = SymbolTable.GetInstance().CurrentScope.FileName;
            foreach (var parent in Parents)
            {
                var parentName = string.Join(".", parent.Identifiers.Identifiers);
                var parentType = SymbolTable.GetInstance().FindType(parentName);

                if (parentType == null)
                    throw new SemanticException($"Type '{parentName}' at row {Row} column {Col} in file {currentFile} does not exist.");

                if(parentsDictionary.ContainsKey(parentName))
                    throw new SemanticException($"Interface '{parentName}' at row {Row} column {Col} in file {currentFile} is already listed at the interface list.");

                if(!(parentType is InterfaceDeclaration))
                    throw new SemanticException($"Type '{parentName}' in interface list at row {Row} column {Col} in file {currentFile} is not an interface.");

                parentsDictionary[parentName] = parentType;
            }

            foreach (var method in Methods)
            {
                if(method.Type.EvaluateType() == null)
                    throw new SemanticException($"Method return type at row {method.Row} column {method.Col} in file {currentFile} does not exist.");

                foreach (var param in method.Params)
                {
                    var paramType = param.Type.EvaluateType();
                    if (paramType == null)
                        throw new SemanticException($"Parameter type at row {param.Row} column {param.Col} " +
                                                    $"in file {currentFile} does not exist.");
                }

                var methodSignature = CompilerUtilities.GenerateMethodSignature(method.Identifier, method.Params);
                if(methodsDictionary.ContainsKey(methodSignature))
                    throw new SemanticException($"Type '{Identifier}' already defines a member called '{method.Identifier}' with same parameter types at row {method.Row} column {method.Col} in file {currentFile}");

                methodsDictionary[methodSignature] = methodSignature;
            }

            var circularList = new Stack<string>();
            CheckCircularInheritance(this, circularList);
        }

        private void CheckCircularInheritance(InterfaceDeclaration Interface, Stack<string> circularList)
        {
            foreach (var parent in Interface.Parents)
            {
                var parentName = string.Join(".", parent.Identifiers.Identifiers);
                if(circularList.Contains(parentName))
                    throw new SemanticException($"Inherited interface '{parentName}' causes a cycle in the interface hierarchy of '{Interface.Identifier}' at row {Interface.Row} column {Interface.Col} in file {SymbolTable.GetInstance().CurrentScope.FileName}.");

                circularList.Push(Interface.Identifier);
                var parentInterface = (InterfaceDeclaration) SymbolTable.GetInstance().FindType(parentName);
                CheckCircularInheritance(parentInterface, circularList);
                circularList.Pop();
            }
        }

        public override string GenerateCode()
        {
            ValidateSemantic();
            return "";
        }
    }
}
