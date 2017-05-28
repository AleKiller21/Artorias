using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private EnumDeclaration EnumDeclaration()
        {
            var enumDeclaration = new EnumDeclaration();

            if (!CheckTokenType(TokenType.RwEnum))
                throw new MissingEnumKeywordException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            enumDeclaration.Identifier = _token.Lexeme;
            NextToken();
            enumDeclaration.Members = EnumBody();
            OptionalBodyEnd();

            return enumDeclaration;
        }

        private List<EnumMember> EnumBody()
        {
            if(!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new MissingCurlyBraceOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var enumMembers = OptionalAssignableIdentifiersList();
            if (!CheckTokenType(TokenType.CurlyBraceClose))
                throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            return enumMembers;
        }

        private List<EnumMember> OptionalAssignableIdentifiersList()
        {
            if (CheckTokenType(TokenType.Id))
            {
                var enumMember = new EnumMember {Identifier = _token.Lexeme};
                NextToken();
                return AssignmentOptions(enumMember);
            }
            else
            {
                return new List<EnumMember>();
            }
        }

        private List<EnumMember> AssignmentOptions(EnumMember enumMember)
        {
            if (CheckTokenType(TokenType.Comma))
            {
                //TODO Asignarle a enumMember.value un tipo Expression con un valor secuencial
                var enumMemberList = OptionalAssignableIdentifiersListPrime();
                enumMemberList.Insert(0, enumMember);

                return enumMemberList;
            }
            if (CheckTokenType(TokenType.OpAssignment))
            {
                NextToken();
                //TODO enumMember.value = Expression()
                Expression();
                var enumMemberList = OptionalAssignableIdentifiersListPrime();
                enumMemberList.Insert(0, enumMember);

                return enumMemberList;
            }

            else
            {
                return new List<EnumMember> {enumMember};
            }
        }

        private List<EnumMember> OptionalAssignableIdentifiersListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                return OptionalAssignableIdentifiersList();
            }
            else
            {
                return new List<EnumMember>();
            }
        }
    }
}
