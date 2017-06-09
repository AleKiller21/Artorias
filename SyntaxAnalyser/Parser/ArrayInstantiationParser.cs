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

        private List<int> OptionalRankSpecifierList()
        {
            if (CheckTokenType(TokenType.SquareBracketOpen))
            {
                var commas = RankSpecifier();
                var rankSpecifierCommas = OptionalRankSpecifierList();

                rankSpecifierCommas.Insert(0, commas);
                return rankSpecifierCommas;
            }
            else
            {
                return new List<int>();
            }
        }

        private int RankSpecifier()
        {
            if(!CheckTokenType(TokenType.SquareBracketOpen))
                throw new SquareBracketOpenExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var commas = OptionalCommaList();

            if(!CheckTokenType(TokenType.SquareBracketClose))
                throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();

            return commas;
        }

        private int OptionalCommaList()
        {
            return CheckTokenType(TokenType.Comma) ? CommaList() : 0;
        }

        private int CommaList()
        {
            var commas = 0;

            if (!CheckTokenType(TokenType.Comma))
                throw new CommaExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            commas++;
            commas += OptionalCommaList();

            return commas;
        }
    }
}
