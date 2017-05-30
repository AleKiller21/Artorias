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
        private Expression UnaryExpression()
        {
            if (IsExpressionUnaryOperator() || IsIncrementDecrementOperator())
            {
                var expression = ExpressionUnaryOperatorOrIncrementDecrement();
                expression.Operand = UnaryExpression();
                return expression;
            }

            if (IsPrimaryExpression() || CheckTokenType(TokenType.Id))
            {
                return PrimaryExpressionOrIdentifier();
            }

            throw new ParserException($"Unary expression token expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private bool IsPrimaryExpression()
        {
            return CheckTokenType(TokenType.RwNew) ||
                    IsBuiltInType() ||
                    CheckTokenType(TokenType.ParenthesisOpen) ||
                    IsLiteral() || CheckTokenType(TokenType.RwThis);
        }

        private PrimaryExpressionOrIdentifier PrimaryExpressionOrIdentifier()
        {
            if (IsPrimaryExpression()) return PrimaryExpression();
            if (CheckTokenType(TokenType.Id))
            {
                var primaryExpressionIdentifier = new PrimaryExpressionIdentifier(_token.Lexeme);
                NextToken();
                primaryExpressionIdentifier.PrimeExpression = PrimaryExpressionPrime();

                return primaryExpressionIdentifier;
            }

            else throw new ParserException($"Primary expression or identifier token expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private PrimaryExpression PrimaryExpression()
        {
            var expression = PrimaryExpressionBase();
            expression.PrimeExpression = PrimaryExpressionPrime();

            return expression;
        }

        private PrimaryExpression PrimaryExpressionBase()
        {
            if (CheckTokenType(TokenType.RwNew))
            {
                NextToken();
                var instance = new NewInstanceExpression();
                NonArrayType(instance.InstanceType);
                instance.Options = InstanceOptions();

                return instance;
            }

            if (IsBuiltInType())
            {
                return new PrimaryExpression{Base = new BuiltInTypeExpression(BuiltInType()) };
            }
            if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                return new PrimaryExpression { Base = CastOrParenthesizedExpression() };
            }
            if (IsLiteral())
            {
                return new PrimaryExpression { Base = Literal() };
            }
            if (CheckTokenType(TokenType.RwThis))
            {
                NextToken();
                return new PrimaryExpression { Base = new ThisExpression() };
            }
            
            throw new ParserException($"Primary expression base token expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

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
            else
            {
                return new List<DataType>();
            }
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

        private List<PrimaryExpressionPrime> PrimaryExpressionPrime()
        {
            if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                NextToken();
                var primeExpression = new PrimaryExpressionPrime{ ArgumentList = ArgumentList() };
                if(!CheckTokenType(TokenType.ParenthesisClose))
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
                if(!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                var primeExpressions = PrimaryExpressionPrime();

                primeExpressions.Insert(0, primeExpression);
                return primeExpressions;
            }

            if (CheckTokenType(TokenType.MemberAccess))
            {
                var primeExpression = new PrimaryExpressionPrime
                {
                    IdentifierAttribute = new QualifiedIdentifier{ Identifiers = IdentifierAttribute() }
                };

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

        private Expression CastOrParenthesizedExpression()
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
