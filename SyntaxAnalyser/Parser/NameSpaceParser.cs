using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes;
using SyntaxAnalyser.Nodes.Namespaces;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private void OptionalNameSpaceMemberDeclaration(NamesapceDeclaration Namespace)
        {
            if(CheckTokenType(TokenType.RwNameSpace) ||
                HasEncapsulationModifier() || IsGroupDeclaration())
            {
                NamespaceMemberDeclaration(Namespace);
            }
            else
            {
                //Epsilon
            }
        }

        private void NamespaceMemberDeclaration(NamesapceDeclaration Namespace)
        {
            if (CheckTokenType(TokenType.RwNameSpace))
            {
                NamespaceDeclaration(Namespace);
                OptionalNameSpaceMemberDeclaration(Namespace);
            }
            else if (HasEncapsulationModifier() || IsGroupDeclaration())
            {
                Namespace.TypeDeclarations.AddRange(TypeDeclarationList());
                OptionalNameSpaceMemberDeclaration(Namespace);
            }
            else
            {
                throw new ParserException($"'namespace' keyword or EncapsulationModifier or GroupDeclaration token expected at row {GetTokenRow()} column {GetTokenColumn()}.");
            }
        }

        private void NamespaceDeclaration(NamesapceDeclaration Namespace)
        {
            var row = GetTokenRow();
            var column = GetTokenColumn();
            if (!CheckTokenType(TokenType.RwNameSpace))
                throw new MissingNamespaceKeywordException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var newNamespace = new NamesapceDeclaration(row, column) {NamespaceIdentifier = QualifiedIdentifier()};
            NamespaceBody(newNamespace);
            Namespace.NamespaceDeclarations.Add(newNamespace);
        }

        private void NamespaceBody(NamesapceDeclaration Namespace)
        {
            if(!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new MissingCurlyBraceOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Namespace.UsingDirectives = OptionalUsingDirective();
            OptionalNameSpaceMemberDeclaration(Namespace);

            if(!CheckTokenType(TokenType.CurlyBraceClose))
                throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }
    }
}
