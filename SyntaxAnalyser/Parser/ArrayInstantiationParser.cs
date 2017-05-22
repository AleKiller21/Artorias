using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private void OptionalFuncOrArrayCall()
        {
            if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                NextToken();
                ArgumentList();
                if(!CheckTokenType(TokenType.ParenthesisClose))
                    throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }

            else if (CheckTokenType(TokenType.SquareBracketOpen)) OptionalAccessArrayList();
            else
            {
                //Epsilon
            }
        }

        private void OptionalAccessArrayList()
        {
            if (CheckTokenType(TokenType.SquareBracketOpen))
            {
                NextToken();
                ExpressionList();
                if (!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                OptionalAccessArrayList();
            }

            else
            {
                //Epsilon
            }
        }

        private void ArrayType()
        {
            if (IsBuiltInType())
            {
                BuiltInType();
                RankSpecifierList();
            }

            else if (CheckTokenType(TokenType.Id))
            {
                QualifiedIdentifier();
                RankSpecifierList();
            }

            else throw new ParserException($"BuiltInType or identifier token expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private void RankSpecifierList()
        {
            RankSpecifier();
            OptionalRankSpecifierList();
        }

        private void OptionalRankSpecifierList()
        {
            if(CheckTokenType(TokenType.SquareBracketOpen)) RankSpecifierList();
            else
            {
                //Epsilon
            }
        }

        private void RankSpecifier()
        {
            if(!CheckTokenType(TokenType.SquareBracketOpen))
                throw new SquareBracketOpenExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            OptionalCommaList();

            if(!CheckTokenType(TokenType.SquareBracketClose))
                throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }

        private void OptionalCommaList()
        {
            if (CheckTokenType(TokenType.Comma)) CommaList();
            else
            {
                //Epsilon
            }
        }

        private void CommaList()
        {
            if (!CheckTokenType(TokenType.Comma))
                throw new CommaExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            OptionalCommaList();
        }
    }
}
