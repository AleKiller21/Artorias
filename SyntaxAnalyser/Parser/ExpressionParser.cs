using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions;
using SyntaxAnalyser.Nodes.Expressions.Unary.PrimaryExpressions.InstanceExpression;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private bool IsPrimaryExpressionPrime()
        {
            return CheckTokenType(TokenType.ParenthesisOpen) || CheckTokenType(TokenType.SquareBracketOpen) ||
                   CheckTokenType(TokenType.MemberAccess) || IsIncrementDecrementOperator();
        }

        private bool IsPrimaryExpression()
        {
            return CheckTokenType(TokenType.RwNew) || CheckTokenType(TokenType.RwBase) ||
                   CheckTokenType(TokenType.RwThis) ||
                   CheckTokenType(TokenType.ParenthesisOpen) || CheckTokenType(TokenType.Id) || IsLiteral() ||
                   IsBuiltInType();
        }

        private Expression UnaryExpression()
        {
            if (IsExpressionUnaryOperator() || IsIncrementDecrementOperator())
            {
                var expression = ExpressionUnaryOperatorOrIncrementDecrement();
                expression.Operand = UnaryExpression();
                return expression;
            }

            if (IsPrimaryExpression()) return PrimaryExpression();

            throw new ParserException($"Unary expression token expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private PrimaryExpression PrimaryExpression()
        {
            var expression = PrimaryExpressionPrimePrime();
            expression.PrimaryExpressionPrime = PrimaryExpressionPrime();

            return expression;
        }

        private PrimaryExpression PrimaryExpressionPrimePrime()
        {
            if (CheckTokenType(TokenType.RwNew))
            {
                NextToken();
                var instance = new NewInstanceExpression();
                NonArrayType(instance.InstanceType);
                instance.Options = InstanceOptions();

                return new PrimaryExpression { PrimaryExpressionPrimePrime = instance };
            }

            if (CheckTokenType(TokenType.RwBase))
            {
                NextToken();
                return new PrimaryExpression { PrimaryExpressionPrimePrime = BaseAccess() };
            }

            if (CheckTokenType(TokenType.RwThis))
            {
                NextToken();
                return new PrimaryExpression { PrimaryExpressionPrimePrime = new ThisExpression() };
            }
            if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                return new PrimaryExpression { PrimaryExpressionPrimePrime = CastOrParenthesizedExpression() };
            }
            if (CheckTokenType(TokenType.Id))
            {
                var idExpression = new IdExpression{Identifier = _token.Lexeme};
                NextToken();
                return new PrimaryExpression{PrimaryExpressionPrimePrime = idExpression};
            }
            if (IsLiteral())
            {
                return new PrimaryExpression { PrimaryExpressionPrimePrime = Literal() };
            }
            if (IsBuiltInType())
            {
                //return new PrimaryExpression { Base = new BuiltInTypeExpression(BuiltInType()) };
                //TODO return
                BuiltInTypeAccess(BuiltInType());
            }

            throw new ParserException($"Primary expression token expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private void BuiltInTypeAccess(BuiltInDataType type)
        {
            if (CheckTokenType(TokenType.MemberAccess))
            {
                //TODO return
                NextToken();
                if(!CheckTokenType(TokenType.Id))
                    throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

                NextToken();
                CallAccess();
            }
            else
            {
                //Epsilon
            }
        }

        private List<PrimaryExpressionPrime> PrimaryExpressionPrime()
        {
            if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                NextToken();
                var primeExpression = new PrimaryExpressionPrime { ArgumentList = ArgumentList() };
                if (!CheckTokenType(TokenType.ParenthesisClose))
                    throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                var primeExpressions = PrimaryExpressionPrime();

                primeExpressions.Insert(0, primeExpression);
                return primeExpressions;
            }

            if (CheckTokenType(TokenType.SquareBracketOpen))
            {
                NextToken();
                var primeExpression = new PrimaryExpressionPrime { ExpressionList = ExpressionList() };
                if (!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                var primeExpressions = PrimaryExpressionPrime();

                primeExpressions.Insert(0, primeExpression);
                return primeExpressions;
            }

            if (CheckTokenType(TokenType.MemberAccess))
            {
                NextToken();
                if(!CheckTokenType(TokenType.Id))
                    throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

                var primeExpression = new PrimaryExpressionPrime
                {
                    IdentifierAttribute = new QualifiedIdentifier { Identifiers = new List<string>{ _token.Lexeme } }
                };

                NextToken();
                var primeExpressions = PrimaryExpressionPrime();

                primeExpressions.Insert(0, primeExpression);
                return primeExpressions;
            }

            if (IsIncrementDecrementOperator())
            {
                var primeExpression = new PrimaryExpressionPrime { Operator = IncrementDecrement() };
                var primeExpressions = PrimaryExpressionPrime();

                primeExpressions.Insert(0, primeExpression);
                return primeExpressions;
            }

            return new List<PrimaryExpressionPrime>();
        }

        private BaseAccessExpression BaseAccess()
        {
            if (CheckTokenType(TokenType.MemberAccess))
            {
                NextToken();
                if(!CheckTokenType(TokenType.Id))
                    throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

                var baseExpression = new BaseAccessExpression{Identifier = _token.Lexeme};
                NextToken();
                return baseExpression;
            }
            if (CheckTokenType(TokenType.SquareBracketOpen))
            {
                NextToken();
                var expressionList = ExpressionList();
                if(!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                return new BaseAccessExpression{ExpressionList = expressionList};
            }

            throw new ParserException($"'.' or '[' token expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        //private PrimaryExpressionOrIdentifier PrimaryExpressionOrIdentifier()
        //{
        //    if (IsPrimaryExpression()) return PrimaryExpression();
        //    if (CheckTokenType(TokenType.Id))
        //    {
        //        var primaryExpressionIdentifier = new PrimaryExpressionIdentifier(_token.Lexeme);
        //        NextToken();
        //        primaryExpressionIdentifier.PrimeExpression = PrimaryExpressionPrime();

        //        return primaryExpressionIdentifier;
        //    }

        //    else throw new ParserException($"Primary expression or identifier token expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        //}

        private InstanceOptions InstanceOptions()
        {
            if (CheckTokenType(TokenType.SquareBracketOpen))
            {
                NextToken();
                return InstanceOptions2();
            }

            if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                NextToken();
                var options = new InstanceOptions{ArgumentList = ArgumentList() };
                if(!CheckTokenType(TokenType.ParenthesisClose))
                    throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                return options;
            }

            if (CheckTokenType(TokenType.OpLessThan))
            {
                var options = new InstanceOptions { Generic = Generic() };
                return options;
            }

            throw new ParserException($"'[' or '(' expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private List<DataType> Generic()
        {
            if(!CheckTokenType(TokenType.OpLessThan))
                throw new ParserException($"'<' token expected at row {GetTokenRow()} column {GetTokenColumn()}");

            NextToken();
            var genericTypes = TypeList();
            if(!CheckTokenType(TokenType.OpGreaterThan))
                throw new ParserException($"'>' token expected at row {GetTokenRow()} column {GetTokenColumn()}");
            NextToken();

            return genericTypes;
        }

        private List<DataType> TypeList()
        {
            var type = Type();
            var typeList = TypeListPrime();

            typeList.Insert(0, type);
            return typeList;
        }

        private List<DataType> TypeListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                var type = Type();
                var typeList = TypeListPrime();

                typeList.Insert(0, type);
                return typeList;
            }

            return new List<DataType>();
        }

        private InstanceOptions InstanceOptions2()
        {
            //TODO Return InstanceOptions2 class instance
            if (IsUnaryExpression())
            {
                var expressions = ExpressionList();
                if (!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                OptionalRankSpecifierList();
                OptionalArrayInitializer();

                return new InstanceOptions{ExpressionList = expressions};
            }

            if (CheckTokenType(TokenType.Comma) || CheckTokenType(TokenType.SquareBracketClose))
            {
                OptionalCommaList();
                if (!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                OptionalRankSpecifierList();
                ArrayInitializer();
                return null;
            }

            throw new ParserException($"Unary expression or comma token expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private CastOrParenthesizedExpression CastOrParenthesizedExpression()
        {
            if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var castOrParenthesizedExpression = new CastOrParenthesizedExpression{ParethesisExpression = Expression() };
            if(!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            castOrParenthesizedExpression.NonParenthesisExpression = OptionalExpression();

            return castOrParenthesizedExpression;
        }

        private List<Expression> ExpressionList()
        {
            var expression = Expression();
            var expressions = ExpressionListPrime();

            expressions.Insert(0, expression);
            return expressions;
        }

        private List<Expression> ExpressionListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                return ExpressionList();
            }
            
            return new List<Expression>();
        }

        private Expression Expression()
        {
            return ConditionalExpression();
        }
    }
}
