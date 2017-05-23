using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private bool IsPrimaryExpressionPrime()
        {
            return CheckTokenType(TokenType.ParenthesisOpen) || CheckTokenType(TokenType.SquareBracketOpen) ||
                   CheckTokenType(TokenType.MemberAccess) || IsIncrementDecrementOperator();
        }
        private void UnaryExpression()
        {
            if (IsExpressionUnaryOperator() || IsIncrementDecrementOperator())
            {
                ExpressionUnaryOperatorOrIncrementDecrement();
                UnaryExpression();
            }

            else if (IsPrimaryExpression() || CheckTokenType(TokenType.Id))
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

        private bool IsPrimaryExpression()
        {
            return CheckTokenType(TokenType.RwNew) ||
                    IsBuiltInType() ||
                    CheckTokenType(TokenType.ParenthesisOpen) ||
                    IsLiteral() || CheckTokenType(TokenType.RwThis);
        }

        private void PrimaryExpressionOrIdentifier()
        {
            if (IsPrimaryExpression()) PrimaryExpression();
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

            else if (IsBuiltInType()) BuiltInType();
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

            else if (CheckTokenType(TokenType.OpLessThan))
            {
                Generic();
            }

            else
            {
                throw new ParserException($"'[' or '(' expected at row {GetTokenRow()} column {GetTokenColumn()}");
            }
        }

        private void Generic()
        {
            if(!CheckTokenType(TokenType.OpLessThan))
                throw new ParserException($"'<' token expected at row {GetTokenRow()} column {GetTokenColumn()}");

            NextToken();
            TypeList();
            if(!CheckTokenType(TokenType.OpGreaterThan))
                throw new ParserException($"'>' token expected at row {GetTokenRow()} column {GetTokenColumn()}");
            NextToken();
        }

        private void TypeList()
        {
            Type();
            TypeListPrime();
        }

        private void TypeListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                Type();
                TypeListPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private void InstanceOptions2()
        {
            if (IsUnaryExpression())
            {
                ExpressionList();
                if (!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                OptionalRankSpecifierList();
                OptionalArrayInitializer();
            }

            else if (CheckTokenType(TokenType.Comma) || CheckTokenType(TokenType.SquareBracketClose))
            {
                OptionalCommaList();
                if (!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                OptionalRankSpecifierList();
                ArrayInitializer();
            }
            else throw new ParserException($"Unary expression or comma token expected at row {GetTokenRow()} column {GetTokenColumn()}.");
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
            OptionalExpression();
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
