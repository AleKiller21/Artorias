using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private void InterfaceDeclaration()
        {
            if (!CheckTokenType(TokenType.RwInterface))
                throw new MissingInterfaceKeywordException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            NextToken();
            InheritanceBase();
            InterfaceBody();
            OptionalBodyEnd();
        }

        private void InterfaceBody()
        {
            if(!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new MissingCurlyBraceOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            InterfaceMethodDeclarationList();
            if(!CheckTokenType(TokenType.CurlyBraceClose))
                throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }

        private void InterfaceMethodDeclarationList()
        {
            if (IsTypeOrVoid())
            {
                InterfaceMethodHeader();
                if(!CheckTokenType(TokenType.EndStatement))
                    throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

                NextToken();
                InterfaceMethodDeclarationList();
            }

            else
            {
                //Epsilon
            }
        }

        private void InterfaceMethodHeader()
        {
            TypeOrVoid();
            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            FixedParameters();

            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }

        private void InheritanceBase()
        {
            if (CheckTokenType(TokenType.Colon))
            {
                NextToken();
                IdentifiersList();
            }

            else
            {
                //Epsilon
            }
        }
    }
}
