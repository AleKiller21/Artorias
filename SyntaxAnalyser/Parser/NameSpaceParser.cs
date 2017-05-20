using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private void OptionalNameSpaceMemberDeclaration()
        {
            if(CheckTokenType(TokenType.RwNameSpace)) NamespaceMemberDeclaration();
            else
            {
                //Epsilon
            }
        }

        private void NamespaceMemberDeclaration()
        {
            if (CheckTokenType(TokenType.RwNameSpace))
            {
                NamespaceDeclaration();
                OptionalNameSpaceMemberDeclaration();
            }
            else if(CheckTokenType(TokenType.RwPublic) || CheckTokenType(TokenType.RwPrivate) || CheckTokenType(TokenType.RwProtected))
                TypeDeclarationList();
        }

        private void NamespaceDeclaration()
        {
            if(!CheckTokenType(TokenType.RwNameSpace))
                throw new ParserException($"namespace keyword expected at row {GetTokenRow()} column {GetTokenColumn()}");

            _token = GetNextToken();
            if(!CheckTokenType(TokenType.Id))
                throw new ParserException($"Id token expected at row {GetTokenRow()} column {GetTokenColumn()}");

            _token = GetNextToken();
            IdentifierAttribute();
            NamespaceBody();
        }

        private void NamespaceBody()
        {
            if(!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new ParserException($"OpenCurlyBrace token expected at row {GetTokenRow()} column {GetTokenColumn()}");

            _token = GetNextToken();
            OptionalUsingDirective();
            OptionalNameSpaceMemberDeclaration();

            if(!CheckTokenType(TokenType.CurlyBraceClose))
                throw new ParserException($"CurlyBraceClosed token expected at row {GetTokenRow()} column {GetTokenColumn()}");

            _token = GetNextToken();
        }
    }
}
