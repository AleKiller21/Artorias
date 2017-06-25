using System.Collections.Generic;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Interfaces;
using SyntaxAnalyser.Nodes.Types;
using SyntaxAnalyser.TablesMetadata;
using SyntaxAnalyser.TablesMetadata.Symbols;
using SyntaxAnalyser.Utilities;

namespace SyntaxAnalyser.Nodes.Classes
{
    class ClassDeclaration : TypeDeclaration
    {
        public bool IsAbstract;
        public List<QualifiedIdentifier> Parents;
        public List<ClassMemberDeclaration> Members;

        public override void ValidateSemantic()
        {
            if(Modifier != AccessModifier.Public && Modifier != AccessModifier.None)
                throw new SemanticException($"{Identifier} access modifier is invalid at row {Row} column {Col} in file {SymbolTable.GetInstance().CurrentScope.FileName}.");

            CheckParents();
            var circularList = new Stack<string>();
            CheckClassCircularInheritance(this, circularList);
            CheckFields();
        }

        private void CheckFields()
        {
            foreach (var fieldDeclaration in Members)
            {
                if(!(fieldDeclaration is FieldDeclaration)) continue;

                var field = (FieldDeclaration) fieldDeclaration;
                var fieldType = CheckFieldType(field);
                CheckFieldDuplication(field);

                SymbolTable.GetInstance().CurrentScope.InsertSymbol(field.Identifier, new FieldAttributes(field, fieldType));
            }
        }

        private void CheckFieldDuplication(FieldDeclaration field)
        {
            SymbolTable.GetInstance()
                .CurrentScope.CheckSymbolDuplication(field.Identifier, field.Row, field.Col,
                    $"The type '{Identifier}' already contains a definition for ");
        }

        private Type CheckFieldType(FieldDeclaration field)
        {
            var fieldType = field.Type.EvaluateType();
            if (fieldType == null)
                throw new GeneralSemanticException(field.Row, field.Col, SymbolTable.GetInstance().CurrentScope.FileName, CompilerUtilities.GetQualifiedName(field.Type.CustomTypeName));

            if(fieldType is VoidType)
                throw new SemanticException($"Field type cannot be 'void' at row {field.Row} column {field.Col} in file {CompilerUtilities.FileName}.");

            return fieldType;
        }

        private void CheckParents()
        {
            var baseClassCounter = 0;
            var flag = false;
            foreach (var parent in Parents)
            {
                var parentName = CompilerUtilities.GetQualifiedName(parent);
                var parentType = CheckParentType(parentName);

                flag = CheckInheritanceOrder(parentType, parentName, flag);
                baseClassCounter = CheckBaseClasCounter(baseClassCounter, parentType);
                CheckInterfaceParentDuplication(parentName, parentType);

                if(parentType is ClassDeclaration)
                    SymbolTable.GetInstance().CurrentScope.InsertSymbol(parentName, new ClassAttribute(parentType));
                else
                    SymbolTable.GetInstance().CurrentScope.InsertSymbol(parentName, new InterfaceAttribute(parentType));
            }
        }

        private Type CheckParentType(string parentName)
        {
            var currentFile = SymbolTable.GetInstance().CurrentScope.FileName;
            var parentType = SymbolTable.GetInstance().FindType(parentName);

            if (parentType == null)
                throw new SemanticException($"Type '{parentName}' at row {Row} column {Col} in file {currentFile} does not exist.");

            if(!(parentType is ClassDeclaration) && !(parentType is InterfaceDeclaration))
                throw new SemanticException($"Type '{Identifier}' at row {Row} column {Col} in file {currentFile} can only inherit from another class or interface. '{parentType}' is none of those.");

            return parentType;
        }

        private bool CheckInheritanceOrder(Type parentType, string parentName, bool flag)
        {
            if (parentType is InterfaceDeclaration) return true;
            if (parentType is ClassDeclaration && flag)
                throw new SemanticException($"Base class '{parentName}' must come before any interface");

            return false;
        }

        private int CheckBaseClasCounter(int counter, Type parentType)
        {
            if (parentType is ClassDeclaration) counter++;
            if (counter == 2)
                throw new SemanticException($"Class '{Identifier}' cannot have multiple base classes at row {Row} column {Col} in file" +
                                            $"{SymbolTable.GetInstance().CurrentScope.FileName}");
            return counter;
        }

        private void CheckInterfaceParentDuplication(string parentName, Type parentType)
        {
            var currentFile = SymbolTable.GetInstance().CurrentScope.FileName;
            var errMessage =
                $"'{parentName}' at row {Row} column {Col} in file {currentFile} is already listed at the interface list.";
            SymbolTable.GetInstance().CurrentScope.CheckSymbolDuplication(parentName, Row, Col, errMessage);
        }

        private void CheckClassCircularInheritance(ClassDeclaration Class, Stack<string> circularList)
        {
            foreach (var parent in Class.Parents)
            {
                var parentName = CompilerUtilities.GetQualifiedName(parent);
                var parentType = SymbolTable.GetInstance().FindType(parentName);
                if (parentType is InterfaceDeclaration)
                {
                    InterfaceDeclaration.CheckInterfaceCircularInheritance((InterfaceDeclaration)parentType,
                        circularList);
                }
                else if (parentType == null || !(parentType is ClassDeclaration))
                    throw new SemanticException(
                        $"The type '{parentName}' at {Class.Row} column {Class.Col} in file {SymbolTable.GetInstance().CurrentScope.FileName} is not a class);");

                else
                {
                    if(circularList.Contains(parentName))
                        throw new SemanticException($"Circular base class involving '{Class.Identifier}' and '{parentName}' at row {Class.Row} column {Class.Col} in file {SymbolTable.GetInstance().CurrentScope.FileName}.");

                    circularList.Push(Class.Identifier);
                    CheckClassCircularInheritance((ClassDeclaration)parentType, circularList);
                    circularList.Pop();
                }
            }
        }

        public override string GenerateCode()
        {
            ValidateSemantic();
            return "";
        }
    }
}
