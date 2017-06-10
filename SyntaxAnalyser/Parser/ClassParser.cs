using System.Collections.Generic;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Classes;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Statements;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private ClassDeclaration ClassDeclaration()
        {
            var classDeclaration = new ClassDeclaration();

            classDeclaration.IsAbstract = ClassModifier();
            if (!CheckTokenType(TokenType.RwClass))
                throw new ClassKeywordExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();

            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            classDeclaration.Identifier = _token.Lexeme;
            NextToken();
            classDeclaration.Parents = InheritanceBase();
            classDeclaration.Members = ClassBody();
            OptionalBodyEnd();

            return classDeclaration;
        }

        private List<ClassMemberDeclaration> ClassBody()
        {
            if(!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new MissingCurlyBraceOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var memberList = OptionalClassMemberDeclarationList();

            if (!CheckTokenType(TokenType.CurlyBraceClose))
                throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            return memberList;
        }

        private List<ClassMemberDeclaration> OptionalClassMemberDeclarationList()
        {
            if (HasEncapsulationModifier() || HasOptionalModifier() || IsTypeOrVoid())
            {
                var memberDeclarations = new List<ClassMemberDeclaration>();
                var memberDeclaration = ClassMemberDeclaration();
                
                memberDeclarations.Add(memberDeclaration);
                var declaration = memberDeclaration as FieldDeclaration;
                if (declaration != null)
                {
                    foreach (var field in declaration.Declarators)
                    {
                        field.AccessModifier = memberDeclaration.AccessModifier;
                        field.OptionalModifier = memberDeclaration.OptionalModifier;
                        field.Type = memberDeclaration.Type;
                    }
                    memberDeclarations.AddRange(declaration.Declarators);
                    declaration.Declarators.Clear();
                }

                memberDeclarations.AddRange(OptionalClassMemberDeclarationList());
                return memberDeclarations;
            }

            else
            {
                return new List<ClassMemberDeclaration>();
            }
        }

        private ClassMemberDeclaration ClassMemberDeclaration()
        {
            var accessModifier = EncapsulationModifier();
            var memberDeclaration = ClassMemberDeclarationOptions();

            memberDeclaration.AccessModifier = accessModifier;
            return memberDeclaration;
        }

        private ClassMemberDeclaration ClassMemberDeclarationOptions()
        {
            var optionalModifier = OptionalModifier();
            var type = TypeOrVoid();
            var memberDeclaration = ClassMemberDeclarationOptionsPrime();

            memberDeclaration.OptionalModifier = optionalModifier;
            memberDeclaration.Type = type;

            return memberDeclaration;
        }

        private ClassMemberDeclaration ClassMemberDeclarationOptionsPrime()
        {
            if (CheckTokenType(TokenType.Id))
            {
                var identifier = _token.Lexeme;
                NextToken();
                return FieldOrMethodDeclaration(identifier);
            }
            if (CheckTokenType(TokenType.ParenthesisOpen)) return ConstructorDeclaration();

            throw new ParserException($"OpenParenthesis or identifier token expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private ConstructorDeclaration ConstructorDeclaration()
        {
            var constructor = ConstructorDeclarator();
            constructor.Statements = MaybeEmptyBlock();

            return constructor;
        }

        private ConstructorDeclaration ConstructorDeclarator()
        {
            if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var constructor = new ConstructorDeclaration { Params = FixedParameters() };

            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            ConstructorInitializer();

            return constructor;
        }

        private void ConstructorInitializer()
        {
            //TODO Tratar con las llamadas a la clase padre
            if (CheckTokenType(TokenType.Colon))
            {
                NextToken();

                if (!CheckTokenType(TokenType.RwBase))
                    throw new BaseKeywordExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();

                if(!CheckTokenType(TokenType.ParenthesisOpen))
                    throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

                NextToken();
                ArgumentList();

                if(!CheckTokenType(TokenType.ParenthesisClose))
                    throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }

            else
            {
                //Epsilon
            }
        }

        private List<Expression> ArgumentList()
        {
            if (IsUnaryExpression())
            {
                var expression = Expression();
                var expressionList = ArgumentListPrime();

                expressionList.Insert(0, expression);
                return expressionList;
            }

            return new List<Expression>();
        }

        private List<Expression> ArgumentListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                var expression = Expression();
                var expressionList = ArgumentListPrime();

                expressionList.Insert(0, expression);
                return expressionList;
            }

            return new List<Expression>();
        }

        private OptionalModifier OptionalModifier()
        {
            if (CheckTokenType(TokenType.RwStatic))
            {
                NextToken();
                return Nodes.Classes.OptionalModifier.Static;
            }
            if (CheckTokenType(TokenType.RwVirtual))
            {
                NextToken();
                return Nodes.Classes.OptionalModifier.Virtual;
            }
            if (CheckTokenType(TokenType.RwOverride))
            {
                NextToken();
                return Nodes.Classes.OptionalModifier.Override;
            }
            if (CheckTokenType(TokenType.RwAbstract))
            {
                NextToken();
                return Nodes.Classes.OptionalModifier.Abstract;
            }
            
            return Nodes.Classes.OptionalModifier.None;
        }

        private ClassMemberDeclaration FieldOrMethodDeclaration(string identifier)
        {
            if (CheckTokenType(TokenType.OpAssignment) || CheckTokenType(TokenType.Comma) ||
                CheckTokenType(TokenType.EndStatement))
            {
                var fieldDeclaration = FieldDeclaration();
                fieldDeclaration.Identifier = identifier;

                return fieldDeclaration;
            }

            if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                var methodDeclaration = MethodDeclaration();
                methodDeclaration.Identifier = identifier;
                return methodDeclaration;
            }

            throw new ParserException($"Field or method declaration expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private FieldDeclaration FieldDeclaration()
        {
            var fieldDeclaration = new FieldDeclaration();

            fieldDeclaration.Value = VariableAssigner();
            fieldDeclaration.Declarators = VariableDeclaratorListPrime();

            if(!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();

            return fieldDeclaration;
        }

        private VariableInitializer VariableAssigner()
        {
            if (CheckTokenType(TokenType.OpAssignment))
            {
                NextToken();
                return VariableInitializer();
            }

            return new VariableInitializer();
        }

        private VariableInitializer VariableInitializer()
        {
            var variableInitializer = new VariableInitializer();
            if (IsUnaryExpression())
            {
                variableInitializer.Expression = Expression();

                return variableInitializer;
            }
            if (CheckTokenType(TokenType.CurlyBraceOpen))
            {
                variableInitializer.ArrayInitializers = ArrayInitializer();
                return variableInitializer;
            }

            throw new ParserException($"OpenCurlyBrace token or expression expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private List<VariableInitializer> ArrayInitializer()
        {
            if(!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new MissingCurlyBraceOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var arrayInitializer = ArrayInitializerPrime();

            if(!CheckTokenType(TokenType.CurlyBraceClose))
                throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            return arrayInitializer;
        }

        private void OptionalArrayInitializer()
        {
            //TODO Borrar ya que no se usa en la gramatica
            if(CheckTokenType(TokenType.CurlyBraceOpen))
                ArrayInitializer();
            else
            {
                //Epsilon
            }
        }

        private List<VariableInitializer> ArrayInitializerPrime()
        {
            if (IsUnaryExpression() || CheckTokenType(TokenType.CurlyBraceOpen))
            {
                var variableInitializer = VariableInitializerList();
                OptionalComma();
                return variableInitializer;
            }

            return new List<VariableInitializer>();
        }

        private void OptionalComma()
        {
            if(CheckTokenType(TokenType.Comma)) NextToken();
            else
            {
                //Epsilon
            }
        }

        private List<VariableInitializer> VariableInitializerList()
        {
            var initializer = VariableInitializer();
            var initializerList = VariableInitializerListPrime();

            initializerList.Insert(0, initializer);
            return initializerList;
        }

        private List<VariableInitializer> VariableInitializerListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                return VariableInitializerList();
            }

            return new List<VariableInitializer>();
        }

        private List<FieldDeclaration> VariableDeclaratorListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                return VariableDeclaratorList();
            }

            return new List<FieldDeclaration>();
        }

        private List<FieldDeclaration> VariableDeclaratorList()
        {
            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            var field = new FieldDeclaration{Identifier = _token.Lexeme};
            NextToken();
            field.Value = VariableAssigner();
            var declarators = VariableDeclaratorListPrime();

            declarators.Insert(0, field);
            return declarators;
        }

        private ClassMethodDeclaration MethodDeclaration()
        {
            //TODO Semantic: validar que si la clase es abstract, el metodo no debe llevar cuerpo.
            //TODO Semantic: validar que si el metodo no es abstracto, entonces no puede terminar en ';'
             if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            //FixedParameters();
            var methodDeclaration = new ClassMethodDeclaration {Params = FixedParameters()};

            if(!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            methodDeclaration.Statements = MaybeEmptyBlock();

            return methodDeclaration;
        }

        private StatementBlock MaybeEmptyBlock()
        {
            if (CheckTokenType(TokenType.CurlyBraceOpen))
            {
                NextToken();

                var block = new StatementBlock{StatementList = OptionalStatementList() };
                if (!CheckTokenType(TokenType.CurlyBraceClose))
                    throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                return block;
            }
            if (CheckTokenType(TokenType.EndStatement))
            {
                NextToken();
                return new StatementBlock();
            }
            
            throw new ParserException($"CurlyBraceOpen or EndOfStatement token expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private bool ClassModifier()
        {
            if (CheckTokenType(TokenType.RwAbstract))
            {
                NextToken();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
