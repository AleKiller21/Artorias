using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;

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
            else if(HasEncapsulationModifier() || (CheckTokenType(TokenType.RwAbstract) || CheckTokenType(TokenType.RwClass))
                || CheckTokenType(TokenType.RwInterface) || CheckTokenType(TokenType.RwEnum))
                TypeDeclarationList();
        }

        private void NamespaceDeclaration()
        {
            if(!CheckTokenType(TokenType.RwNameSpace))
                throw new MissingNamespaceKeywordException($"namespace keyword expected at row {GetTokenRow()} column {GetTokenColumn()}");

            _token = GetNextToken();
            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException($"Id token expected at row {GetTokenRow()} column {GetTokenColumn()}");

            _token = GetNextToken();
            IdentifierAttribute();
            NamespaceBody();
        }

        private void NamespaceBody()
        {
            if(!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new MissingCurlyBraceOpenException($"OpenCurlyBrace token expected at row {GetTokenRow()} column {GetTokenColumn()}");

            _token = GetNextToken();
            OptionalUsingDirective();
            OptionalNameSpaceMemberDeclaration();

            if(!CheckTokenType(TokenType.CurlyBraceClose))
                throw new MissingCurlyBraceClosedException($"CurlyBraceClosed token expected at row {GetTokenRow()} column {GetTokenColumn()}");

            _token = GetNextToken();
        }
    }
}
