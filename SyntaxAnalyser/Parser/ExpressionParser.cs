using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private void UnaryExpression()
        {
            if (IsExpressionUnaryOperator() || IsIncrementDecrementOperator())
            {
                ExpressionUnaryOperatorOrIncrementDecrement();
                UnaryExpression();
            }

            else if (CheckTokenType(TokenType.RwNew) ||
                     IsBuiltInType() ||
                     CheckTokenType(TokenType.ParenthesisOpen) ||
                     IsLiteral() || CheckTokenType(TokenType.RwThis) || CheckTokenType(TokenType.Id))
            {
                PrimaryExpressionOrIdentifier();
            }

            else throw new ParserException($"Unary expression token expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void PrimaryExpression()
        {
            PrimaryExpressionBase();
            PrimaryExpressionPrime();
        }

        private void PrimaryExpressionOrIdentifier()
        {
            if (CheckTokenType(TokenType.RwNew) ||
                IsBuiltInType() ||
                CheckTokenType(TokenType.ParenthesisOpen) ||
                IsLiteral() || CheckTokenType(TokenType.RwThis))
            {
                PrimaryExpression();
            }

            else if (CheckTokenType(TokenType.Id))
            {
                NextToken();
                PrimaryExpressionPrime();
            }

            else throw new ParserException($"Primary expression or identifier token expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void PrimaryExpressionBase()
        {
            if (CheckTokenType(TokenType.RwNew))
            {
                NextToken();
                NonArrayType();
                InstanceOptions();
            }

            else if (IsBuiltInType())
            {
                BuiltInType();
                if(!CheckTokenType(TokenType.MemberAccess))
                    throw new ParserException($"Member access operator expected at row {GetTokenRow()} column {GetTokenColumn()}.");

                NextToken();
                if(!CheckTokenType(TokenType.Id))
                    throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }

            else if (CheckTokenType(TokenType.ParenthesisOpen)) CastOrParenthesizedExpression();
            else if(IsLiteral()) Literal();
            else if(CheckTokenType(TokenType.RwThis)) NextToken();
            else throw new ParserException($"Primary expression base token expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private void InstanceOptions()
        {
            if (CheckTokenType(TokenType.SquareBracketOpen))
            {
                NextToken();
                InstanceOptions2();
            }

            else if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                NextToken();
                ArgumentList();
                if(!CheckTokenType(TokenType.ParenthesisClose))
                    throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }
        }

        private void InstanceOptions2()
        {
            //TODO needs expression implementation
            throw new NotImplementedException();
        }

        private void PrimaryExpressionPrime()
        {
            if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                NextToken();
                ArgumentList();
                if(!CheckTokenType(TokenType.ParenthesisClose))
                    throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                PrimaryExpressionPrime();
            }

            else if (CheckTokenType(TokenType.SquareBracketOpen))
            {
                NextToken();
                ExpressionList();
                if(!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                PrimaryExpressionPrime();
            }

            else if (CheckTokenType(TokenType.MemberAccess))
            {
                IdentifierAttribute();
                PrimaryExpressionPrime();
            }

            else if (IsIncrementDecrementOperator())
            {
                IncrementDecrement();
                PrimaryExpressionPrime();
            }

            else
            {
                //Epsilon
            }
        }

        private void CastOrParenthesizedExpression()
        {
            if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Expression();
            if(!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }

        private void ExpressionList()
        {
            Expression();
            ExpressionListPrime();
        }

        private void ExpressionListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                ExpressionList();
            }
            else
            {
                //Epsilon
            }
        }

        private void Expression()
        {
            ConditionalExpression();
        }
    }
}
