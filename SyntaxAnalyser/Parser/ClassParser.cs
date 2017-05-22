using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private void ClassDeclaration()
        {
            ClassModifier();
            if (!CheckTokenType(TokenType.RwClass))
                throw new ClassKeywordExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();

            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            NextToken();
            InheritanceBase();
            ClassBody();
            OptionalBodyEnd();
        }

        private void ClassBody()
        {
            if(!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new MissingCurlyBraceOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            OptionalClassMemberDeclarationList();

            if (!CheckTokenType(TokenType.CurlyBraceClose))
                throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }

        private void OptionalClassMemberDeclarationList()
        {
            if (HasEncapsulationModifier() || HasOptionalModifier() || IsTypeOrVoid())
            {
                ClassMemberDeclaration();
                OptionalClassMemberDeclarationList();
            }

            else
            {
                //Epsilon
            }
        }

        private void ClassMemberDeclaration()
        {
            EncapsulationModifier();
            ClassMemberDeclarationOptions();
        }

        private void ClassMemberDeclarationOptions()
        {
            OptionalModifier();
            TypeOrVoid();
            ClassMemberDeclarationOptionsPrime();
        }

        private void ClassMemberDeclarationOptionsPrime()
        {
            if (CheckTokenType(TokenType.Id))
            {
                NextToken();
                FieldOrMethodDeclaration();
            }

            else if (CheckTokenType(TokenType.ParenthesisOpen))
                ConstructorDeclaration();

            else throw new ParserException($"OpenParenthesis or identifier token expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private void ConstructorDeclaration()
        {
            ConstructorDeclarator();
            MaybeEmptyBlock();
        }

        private void ConstructorDeclarator()
        {
            if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            FixedParameters();

            if(!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            ConstructorInitializer();
        }

        private void ConstructorInitializer()
        {
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

        private void ArgumentList()
        {
            if (IsUnaryExpression())
            {
                Expression();
                ArgumentListPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private void ArgumentListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                Expression();
                ArgumentListPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private void OptionalModifier()
        {
            if (HasOptionalModifier()) NextToken();
            else
            {
                //Epsilon
            }
        }

        private void FieldOrMethodDeclaration()
        {
            if (CheckTokenType(TokenType.OpAssignment) || CheckTokenType(TokenType.Comma) ||
                CheckTokenType(TokenType.EndStatement))
                FieldDeclaration();

            else if (CheckTokenType(TokenType.ParenthesisOpen))
                MethodDeclaration();

            else throw new ParserException($"Field or method declaration expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private void FieldDeclaration()
        {
            VariableAssigner();
            VariableDeclaratorListPrime();

            if(!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }

        private void VariableAssigner()
        {
            if (CheckTokenType(TokenType.OpAssignment))
            {
                NextToken();
                VariableInitializer();
            }

            else
            {
                //Epsilon
            }
        }

        private void VariableInitializer()
        {
            if (IsUnaryExpression()) Expression();
            else if (CheckTokenType(TokenType.CurlyBraceOpen)) ArrayInitializer();
            else throw new ParserException($"OpenCurlyBrace token or expression expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private void ArrayInitializer()
        {
            if(!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new MissingCurlyBraceOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            OptionalVariableInitializerList();

            if(!CheckTokenType(TokenType.CurlyBraceClose))
                throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }

        private void OptionalArrayInitializer()
        {
            if(CheckTokenType(TokenType.CurlyBraceOpen))
                ArrayInitializer();
            else
            {
                //Epsilon
            }
        }

        private void OptionalVariableInitializerList()
        {
            if (IsUnaryExpression() || CheckTokenType(TokenType.CurlyBraceOpen)) VariableInitializerList();
            else
            {
                //Epsilon
            }
        }

        private void VariableInitializerList()
        {
            VariableInitializer();
            VariableInitializerListPrime();
        }

        private void VariableInitializerListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                VariableInitializerList();
            }

            else
            {
                //Epsilon
            }
        }

        private void VariableDeclaratorListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                VariableDeclaratorList();
            }

            else
            {
                //Epsilon
            }
        }

        private void VariableDeclaratorList()
        {
            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            NextToken();
            VariableAssigner();
            VariableDeclaratorListPrime();
        }

        private void MethodDeclaration()
        {
            //TODO Semantic: validar que si la clase es abstract, el metodo no debe llevar cuerpo.
            //TODO Semantic: validar que si el metodo no es abstracto, entonces no puede terminar en ';'
             if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            FixedParameters();

            if(!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            MaybeEmptyBlock();
        }

        private void MaybeEmptyBlock()
        {
            if (CheckTokenType(TokenType.CurlyBraceOpen))
            {
                NextToken();
                OptionalStatementList();
                if(!CheckTokenType(TokenType.CurlyBraceClose))
                    throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }

            else if(CheckTokenType(TokenType.EndStatement)) NextToken();
            else
                throw new ParserException($"CurlyBraceOpen or EndOfStatement token expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private void ClassModifier()
        {
            if(CheckTokenType(TokenType.RwAbstract)) NextToken();
            else
            {
                //Epsilon
            }
        }
    }
}
