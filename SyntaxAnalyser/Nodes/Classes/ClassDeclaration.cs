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
            CheckFields();
            CheckMethods();
            CheckConstructors();
        }

        private void CheckConstructors()
        {
            foreach (var constructorDeclaration in Members)
            {
                if (!(constructorDeclaration is ConstructorDeclaration)) continue;

                var constructor = (ConstructorDeclaration)constructorDeclaration;
                CheckConstructorsName(constructor);
                CheckConstructorAccessModifier(constructor);
            }
        }

        private void CheckConstructorsName(ConstructorDeclaration constructor)
        {
            var constructorName = CompilerUtilities.GetQualifiedName(constructor.Type.CustomTypeName);
            if (constructorName != Identifier)
                throw new SemanticException($"Constructor '{constructorName}' must have the same name as its enclosing type at row {constructor.Row} column {constructor.Col} in file {CompilerUtilities.FileName}.");
        }

        private void CheckConstructorAccessModifier(ConstructorDeclaration constructor)
        {
            var constructorName = CompilerUtilities.GetQualifiedName(constructor.Type.CustomTypeName);
            if(constructor.OptionalModifier == OptionalModifier.Static && constructor.AccessModifier != AccessModifier.None)
                throw new SemanticException($"Access modifier is not allowed for static constructor '{constructorName}' at row {constructor.Row} column {constructor.Col} in file {CompilerUtilities.FileName}.");
        }

        private void CheckMethods()
        {
            var methodsDictionary = new Dictionary<string, string>();
            foreach (var methodDeclaration in Members)
            {
                if (!(methodDeclaration is ClassMethodDeclaration)) continue;

                var method = (ClassMethodDeclaration) methodDeclaration;

                CheckMethodType(method);
                CheckMethodParameters(method);
                var methodSignature = CompilerUtilities.GenerateMethodSignature(method.Identifier, method.Params);
                CheckMethodDuplication(methodsDictionary, methodSignature, method);
                CheckMethodName(method);
                CheckAbstractVirtualMethodsAccessModifier(method);
                CheckIfAbstractMethodIsInAbstractClass(method);
                CheckAbstractMethodHasBody(method);
                CheckNonAbstractMethodHasBody(method);

                methodsDictionary[methodSignature] = methodSignature;
            }
        }

        private void CheckMethodType(ClassMethodDeclaration method)
        {
            if(method.Type.EvaluateType() == null)
                throw new GeneralSemanticException(method.Row, method.Col, CompilerUtilities.FileName, CompilerUtilities.GetQualifiedName(method.Type.CustomTypeName));
        }

        private void CheckMethodParameters(ClassMethodDeclaration method)
        {
            foreach (var param in method.Params)
            {
                var paramType = param.Type.EvaluateType();
                if (paramType == null)
                    throw new SemanticException($"Parameter type '{CompilerUtilities.GetQualifiedName(param.Type.CustomTypeName)}' at row {param.Row} column {param.Col} in file {CompilerUtilities.FileName} does not exist.");
            }
        }

        private void CheckMethodDuplication(Dictionary<string, string> methodsDictionary, string methodSignature, ClassMethodDeclaration method)
        {
            if (methodsDictionary.ContainsKey(methodSignature))
                throw new SemanticException($"Type '{Identifier}' already defines a member called '{method.Identifier}' with same parameter types at row {method.Row} column {method.Col} in file {CompilerUtilities.FileName}.");
        }

        private void CheckMethodName(ClassMethodDeclaration method)
        {
            if(method.Identifier == Identifier)
                throw new SemanticException($"Method '{method.Identifier}' cannot be named as its enclosing type at row {method.Row} column {method.Col} in file {CompilerUtilities.FileName}.");
        }

        private void CheckAbstractVirtualMethodsAccessModifier(ClassMethodDeclaration method)
        {
            var errorLocation = $"cannot be private at row {method.Row} column {method.Col} in file {CompilerUtilities.FileName}.";

            if(method.OptionalModifier == OptionalModifier.Virtual && method.AccessModifier == AccessModifier.Private)
                throw new SemanticException($"Virtual method '{method.Identifier}' {errorLocation}");

            if(method.OptionalModifier == OptionalModifier.Abstract && method.AccessModifier == AccessModifier.Private)
                throw new SemanticException($"Abstract method '{method.Identifier}' {errorLocation}");
        }

        private void CheckIfAbstractMethodIsInAbstractClass(ClassMethodDeclaration method)
        {
            if(method.OptionalModifier == OptionalModifier.Abstract && !IsAbstract)
                throw new SemanticException($"Abstract method '{method.Identifier}' is abstract but it is contained in non-abstract class '{Identifier}' at row {method.Row} column {method.Col} in file {CompilerUtilities.FileName}.");
        }

        private void CheckAbstractMethodHasBody(ClassMethodDeclaration method)
        {
            if(method.OptionalModifier == OptionalModifier.Abstract && method.Statements != null)
                throw new SemanticException($"Abstract method '{method.Identifier}' cannot declared a body because it is marked as abstract at row {method.Row} column {method.Col} in file {CompilerUtilities.FileName}.");
        }

        private void CheckNonAbstractMethodHasBody(ClassMethodDeclaration method)
        {
            if (method.OptionalModifier != OptionalModifier.Abstract && method.Statements == null)
                throw new SemanticException($"Method '{method.Identifier}' must declare a body because it is not marked as abstract at row {method.Row} column {method.Col} in file {CompilerUtilities.FileName}.");
        }

        private void CheckFields()
        {
            foreach (var fieldDeclaration in Members)
            {
                if(!(fieldDeclaration is FieldDeclaration)) continue;

                var field = (FieldDeclaration) fieldDeclaration;
                var fieldType = CheckFieldType(field);
                CheckFieldDuplication(field);
                CheckFieldName(field);
                CheckFieldOptionalModifier(field);

                SymbolTable.GetInstance().CurrentScope.InsertSymbol(field.Identifier, new FieldAttributes(field, fieldType));
            }
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

        private void CheckFieldDuplication(FieldDeclaration field)
        {
            SymbolTable.GetInstance()
                .CurrentScope.CheckSymbolDuplication(field.Identifier, field.Row, field.Col,
                    $"The type '{Identifier}' already contains a definition for ");
        }

        private void CheckFieldName(FieldDeclaration field)
        {
            if(field.Identifier.Equals(Identifier))
                throw new SemanticException($"field '{field.Identifier}' cannot be named as its enclosing type at row {field.Row} column {field.Col} in file {CompilerUtilities.FileName}.");
        }

        private void CheckFieldOptionalModifier(FieldDeclaration field)
        {
            var errMessage = "";
            if (field.OptionalModifier == OptionalModifier.Abstract)
                errMessage = $"Field '{field.Identifier}' cannot be declared abstract";
            else if (field.OptionalModifier == OptionalModifier.Override)
                errMessage = $"Field '{field.Identifier}' cannot be declared override";
            else if (field.OptionalModifier == OptionalModifier.Virtual)
                errMessage = $"Field '{field.Identifier}' cannot be declared virtual";

            if(errMessage != "")
                throw new SemanticException($"{errMessage} at row {field.Row} column {field.Col} in file {CompilerUtilities.FileName}.");
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

            var circularList = new Stack<string>();
            CheckClassCircularInheritance(this, circularList);
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
                var parentNamespace = SymbolTable.GetInstance().FindTypeNamespace(parentName);
                var type = parentType as InterfaceDeclaration;
                if (type != null)
                {
                    InterfaceDeclaration.CheckInterfaceCircularInheritance(type,
                        circularList);
                }
                else if (!(parentType is ClassDeclaration))
                    throw new SemanticException(
                        $"The type '{parentName}' at {Class.Row} column {Class.Col} in file {SymbolTable.GetInstance().CurrentScope.FileName} is not a class);");

                else
                {
                    var fullTypeName = $"{parentNamespace}.{parentName}";
                    if(circularList.Contains(fullTypeName))
                        throw new SemanticException($"Circular base class involving '{Class.Identifier}' and '{parentName}' at row {Class.Row} column {Class.Col} in file {SymbolTable.GetInstance().CurrentScope.FileName}.");

                    //TODO Hacer lo mismo para CheckInterfaceCircularInheritance
                    var classNamespace = SymbolTable.GetInstance().CurrentScope.CurrentNamespace;
                    circularList.Push($"{classNamespace}.{Class.Identifier}");
                    SymbolTable.GetInstance().PushScope(parentNamespace, CompilerUtilities.FileName);
                    CheckClassCircularInheritance((ClassDeclaration)parentType, circularList);
                    SymbolTable.GetInstance().PopScope();
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
