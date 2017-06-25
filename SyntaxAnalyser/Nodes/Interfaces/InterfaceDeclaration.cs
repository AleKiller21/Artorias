using System;
using System.Collections.Generic;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Types;
using SyntaxAnalyser.TablesMetadata;
using SyntaxAnalyser.Utilities;
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

            if (Modifier != AccessModifier.Public && Modifier != AccessModifier.None)
                throw new SemanticException($"{Identifier} access modifier is invalid at row {Row} column {Col} in file {SymbolTable.GetInstance().CurrentScope.FileName}.");

            foreach (var parent in Parents)
            {
                var parentName = string.Join(".", parent.Identifiers.Identifiers);
                var parentType = CheckParentType(parentName, currentFile);

                CheckParentDuplication(parentName, currentFile, parentsDictionary);
                CheckIfParentIsInterface(parentType, parentName, currentFile);
                parentsDictionary[parentName] = parentType;
            }

            foreach (var method in Methods)
            {
                CheckMethodType(method, currentFile);
                CheckMethodParameters(method, currentFile);
                var methodSignature = CompilerUtilities.GenerateMethodSignature(method.Identifier, method.Params);
                CheckMethodDuplication(methodsDictionary, methodSignature, method, currentFile);

                methodsDictionary[methodSignature] = methodSignature;
            }

            var circularList = new Stack<string>();
            CheckInterfaceCircularInheritance(this, circularList);
        }

        private Type CheckParentType(string parentName, string currentFile)
        {
            var parentType = SymbolTable.GetInstance().FindType(parentName);

            if (parentType == null)
                throw new SemanticException($"Type '{parentName}' at row {Row} column {Col} in file {currentFile} does not exist.");

            return parentType;
        }

        private void CheckParentDuplication(string parentName, string currentFile, Dictionary<string, Type> parentsDictionary)
        {
            if (parentsDictionary.ContainsKey(parentName))
                throw new SemanticException($"Interface '{parentName}' at row {Row} column {Col} in file {currentFile} is already listed at the interface list.");
        }

        private void CheckIfParentIsInterface(Type parentType, string parentName, string currentFile)
        {
            if (!(parentType is InterfaceDeclaration))
                throw new SemanticException($"Type '{parentName}' in interface list at row {Row} column {Col} in file {currentFile} is not an interface.");
        }

        private void CheckMethodType(InterfaceMethodDeclaration method, string currentFile)
        {
            if (method.Type.EvaluateType() == null)
                throw new SemanticException($"Method return type at row {method.Row} column {method.Col} in file {currentFile} does not exist.");
        }

        private void CheckMethodParameters(InterfaceMethodDeclaration method, string currentFile)
        {
            foreach (var param in method.Params)
            {
                var paramType = param.Type.EvaluateType();
                if (paramType == null)
                    throw new SemanticException($"Parameter type at row {param.Row} column {param.Col} " +
                                                $"in file {currentFile} does not exist.");
            }
        }

        private void CheckMethodDuplication(Dictionary<string, string> methodsDictionary, string methodSignature, InterfaceMethodDeclaration method, string currentFile)
        {
            if (methodsDictionary.ContainsKey(methodSignature))
                throw new SemanticException($"Type '{Identifier}' already defines a member called '{method.Identifier}' with same parameter types at row {method.Row} column {method.Col} in file {currentFile}");
        }

        public static void CheckInterfaceCircularInheritance(InterfaceDeclaration Interface, Stack<string> circularList)
        {
            foreach (var parent in Interface.Parents)
            {
                var parentName = CompilerUtilities.GetQualifiedName(parent);
                if(circularList.Contains(parentName))
                    throw new SemanticException($"Inherited interface '{parentName}' causes a cycle in the interface hierarchy of '{Interface.Identifier}' at row {Interface.Row} column {Interface.Col} in file {SymbolTable.GetInstance().CurrentScope.FileName}.");

                circularList.Push(Interface.Identifier);
                var typeFound = SymbolTable.GetInstance().FindType(parentName);
                if(typeFound == null || !(typeFound is InterfaceDeclaration))
                    throw new SemanticException($"The type '{parentName}' at row {Interface.Row} column {Interface.Col} in file {SymbolTable.GetInstance().CurrentScope.FileName} is not an interface.");

                var parentInterface = (InterfaceDeclaration)typeFound;
                CheckInterfaceCircularInheritance(parentInterface, circularList);
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
