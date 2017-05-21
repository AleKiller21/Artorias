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
            if(CheckTokenType(TokenType.RwNameSpace) ||
                HasEncapsulationModifier() || 
                (CheckTokenType(TokenType.RwAbstract) ||
                CheckTokenType(TokenType.RwClass)) || 
                CheckTokenType(TokenType.RwInterface) || 
                CheckTokenType(TokenType.RwEnum))
            {
                NamespaceMemberDeclaration();
            }
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
            else if (HasEncapsulationModifier() || 
                (CheckTokenType(TokenType.RwAbstract) ||
                CheckTokenType(TokenType.RwClass)) ||
                CheckTokenType(TokenType.RwInterface) ||
                CheckTokenType(TokenType.RwEnum))
            {
                TypeDeclarationList();
                OptionalNameSpaceMemberDeclaration();
            }
        }

        private void NamespaceDeclaration()
        {
            if(!CheckTokenType(TokenType.RwNameSpace))
                throw new MissingNamespaceKeywordException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if (!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            NextToken();
            IdentifierAttribute();
            NamespaceBody();
        }

        private void NamespaceBody()
        {
            if(!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new MissingCurlyBraceOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            OptionalUsingDirective();
            OptionalNameSpaceMemberDeclaration();

            if(!CheckTokenType(TokenType.CurlyBraceClose))
                throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }
    }
}
