using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private TypeDeclaration InterfaceDeclaration()
        {
            if (!CheckTokenType(TokenType.RwInterface))
                throw new MissingInterfaceKeywordException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            var interfaceDeclaration = new InterfaceDeclaration();
            interfaceDeclaration.Identifier = _token.Lexeme;

            NextToken();
            interfaceDeclaration.Parents = InheritanceBase();
            interfaceDeclaration.Methods = InterfaceBody();
            OptionalBodyEnd();

            return interfaceDeclaration;
        }

        private List<MethodDeclaration> InterfaceBody()
        {
            if(!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new MissingCurlyBraceOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var methods = InterfaceMethodDeclarationList();
            if(!CheckTokenType(TokenType.CurlyBraceClose))
                throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            return methods;
        }

        private List<MethodDeclaration> InterfaceMethodDeclarationList()
        {
            if (IsTypeOrVoid())
            {
                var methodDeclaration = InterfaceMethodHeader();
                if(!CheckTokenType(TokenType.EndStatement))
                    throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

                NextToken();
                var methodDeclarationList = InterfaceMethodDeclarationList();

                methodDeclarationList.Insert(0, methodDeclaration);
                return methodDeclarationList;
            }

            else
            {
                return new List<MethodDeclaration>();
            }
        }

        private MethodDeclaration InterfaceMethodHeader()
        {
            var methodDeclaration = new MethodDeclaration();
            methodDeclaration.Type = TypeOrVoid();

            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            methodDeclaration.Identifier = _token.Lexeme;
            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            methodDeclaration.Params = FixedParameters();

            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();

            methodDeclaration.Modifier = AccessModifier.Public;
            return methodDeclaration;
        }

        private List<QualifiedIdentifier> InheritanceBase()
        {
            if (CheckTokenType(TokenType.Colon))
            {
                NextToken();
                return IdentifiersList();
            }

            else
            {
                return new List<QualifiedIdentifier>();
            }
        }
    }
}
