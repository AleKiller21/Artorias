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
                HasEncapsulationModifier() || IsGroupDeclaration())
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
            else if (HasEncapsulationModifier() || IsGroupDeclaration())
            {
                TypeDeclarationList();
                OptionalNameSpaceMemberDeclaration();
            }
            else
            {
                throw new ParserException($"'namespace' keyword or EncapsulationModifier or GroupDeclaration token expected at row {GetTokenRow()} column {GetTokenColumn()}.");
            }
        }

        private void NamespaceDeclaration()
        {
            if(!CheckTokenType(TokenType.RwNameSpace))
                throw new MissingNamespaceKeywordException(GetTokenRow(), GetTokenColumn());

            NextToken();
            QualifiedIdentifier();
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
