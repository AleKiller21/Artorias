using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private void EnumDeclaration()
        {
            if (!CheckTokenType(TokenType.RwEnum))
                throw new MissingEnumKeywordException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            NextToken();
            EnumBody();
        }

        private void EnumBody()
        {
            if(!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new MissingCurlyBraceOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if (CheckTokenType(TokenType.Id)) OptionalAssignableIdentifiersList();
            if(!CheckTokenType(TokenType.CurlyBraceClose))
                throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }

        private void OptionalAssignableIdentifiersList()
        {
            if (CheckTokenType(TokenType.Id))
            {
                NextToken();
                if (CheckTokenType(TokenType.Comma) || CheckTokenType(TokenType.OpAssignment))
                    AssignmentOptions();
            }
            else
            {
                //Epsilon
            }
        }

        private void AssignmentOptions()
        {
            if (CheckTokenType(TokenType.Comma)) OptionalAssignableIdentifiersListPrime();
            else if (CheckTokenType(TokenType.OpAssignment))
            {
                NextToken();
                Expression();
                if (CheckTokenType(TokenType.Comma)) OptionalAssignableIdentifiersListPrime();
            }

            else
            {
                //Epsilon
            }
        }

        private void OptionalAssignableIdentifiersListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                if (CheckTokenType(TokenType.Id)) OptionalAssignableIdentifiersList();
            }
            else
            {
                //Epsilon
            }
        }
    }
}
