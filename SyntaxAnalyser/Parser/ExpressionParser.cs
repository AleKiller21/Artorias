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
                var row = GetTokenRow();
                var col = GetTokenColumn();

                NextToken();
                var instance = new NewInstancePrimaryExpression
                {
                    Row = row,
                    Col = col
                };
                NonArrayType(instance.InstanceType);
                instance.Options = InstanceOptions();

                return new PrimaryExpression { PrimaryExpressionPrimePrime = instance };
            }

            if (CheckTokenType(TokenType.RwBase))
            {
                var row = GetTokenRow();
                var col = GetTokenColumn();

                NextToken();
                return new PrimaryExpression
                {
                    Row = row,
                    Col = col,
                    PrimaryExpressionPrimePrime = BaseAccess()
                };
            }

            if (CheckTokenType(TokenType.RwThis))
            {
                var row = GetTokenRow();
                var col = GetTokenColumn();

                NextToken();
                return new PrimaryExpression
                {
                    Row = row,
                    Col = col,
                    PrimaryExpressionPrimePrime = new ThisExpression()
                };
            }
            if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                //TODO SEMANTIC: Check if it's a parenthesized or cast expression
                return new PrimaryExpression
                {
                    Row = GetTokenRow(),
                    Col = GetTokenColumn(),
                    PrimaryExpressionPrimePrime = CastOrParenthesizedExpression()
                };
            }
            if (CheckTokenType(TokenType.Id))
            {
                var row = GetTokenRow();
                var col = GetTokenColumn();

                var idExpression = new IdExpression
                {
                    Identifier = _token.Lexeme,
                    Row = row,
                    Col = col
                };
                NextToken();
                return new PrimaryExpression
                {
                    Row = col,
                    Col = col,
                    PrimaryExpressionPrimePrime = idExpression
                };
            }
            if (IsLiteral())
            {
                return new PrimaryExpression
                {
                    Row = GetTokenRow(),
                    Col = GetTokenColumn(),
                    PrimaryExpressionPrimePrime = Literal()
                };
            }
            if (IsBuiltInType())
            {
                var row = GetTokenRow();
                var col = GetTokenColumn();
                var builtInTypeAccess = new BuiltInTypeAccessExpression
                {
                    Row = row,
                    Col = col,
                    BuiltInType = BuiltInType()
                };
                BuiltInTypeAccess(builtInTypeAccess);
                return new PrimaryExpression
                {
                    Row = row,
                    Col = col,
                    PrimaryExpressionPrimePrime = builtInTypeAccess
                };
            }

            throw new ParserException($"Primary expression token expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private void BuiltInTypeAccess(BuiltInTypeAccessExpression expression)
        {
            if (CheckTokenType(TokenType.MemberAccess))
            {
                NextToken();
                if(!CheckTokenType(TokenType.Id))
                    throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

                expression.Identifier = _token.Lexeme;
                NextToken();
                expression.CallAccess = CallAccess();
            }

            else
            {
                //Epsilon
            }
        }

        private List<PrimaryExpressionPrime> PrimaryExpressionPrime()
        {
            var row = GetTokenRow();
            var col = GetTokenColumn();

            if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                NextToken();
                var primeExpression = new PrimaryExpressionPrime
                {
                    Row = row,
                    Col = col,
                    ArgumentList = ArgumentList()
                };
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
                var primeExpression = new PrimaryExpressionPrime
                {
                    Row = row,
                    Col = col,
                    ExpressionList = ExpressionList()
                };
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
                    Row = row,
                    Col = col,
                    IdentifierAttribute = new QualifiedIdentifier
                    {
                        Identifiers = new IdentifierAttribute(GetTokenRow(), GetTokenColumn())
                        {
                            Identifiers = new List<string> { _token.Lexeme }
                        },
                        Row = GetTokenRow(),
                        Col = GetTokenColumn()
                    }
                };

                NextToken();
                var primeExpressions = PrimaryExpressionPrime();

                primeExpressions.Insert(0, primeExpression);
                return primeExpressions;
            }

            if (IsIncrementDecrementOperator())
            {
                var primeExpression = new PrimaryExpressionPrime
                {
                    Row = row,
                    Col = col,
                    Operator = IncrementDecrement()
                };
                var primeExpressions = PrimaryExpressionPrime();

                primeExpressions.Insert(0, primeExpression);
                return primeExpressions;
            }

            return new List<PrimaryExpressionPrime>();
        }

        private BaseAccessExpression BaseAccess()
        {
            var row = GetTokenRow();
            var col = GetTokenColumn();

            if (CheckTokenType(TokenType.MemberAccess))
            {
                NextToken();
                if(!CheckTokenType(TokenType.Id))
                    throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

                var baseExpression = new BaseAccessExpression
                {
                    Identifier = _token.Lexeme,
                    Row = row,
                    Col = col
                };
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

        private InstanceOptions InstanceOptions()
        {
            var row = GetTokenRow();
            var col = GetTokenColumn();

            if (CheckTokenType(TokenType.SquareBracketOpen))
            {
                NextToken();
                return new InstanceOptions
                {
                    Row = row,
                    Col = col,
                    options = InstanceOptions2()
                };
            }

            if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                NextToken();
                var options = new InstanceOptions
                {
                    Row = row,
                    Col = col,
                    ArgumentList = ArgumentList()
                };
                if(!CheckTokenType(TokenType.ParenthesisClose))
                    throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                return options;
            }

            if (CheckTokenType(TokenType.OpLessThan))
            {
                var options = new InstanceOptions
                {
                    Row = row,
                    Col = col,
                    Generic = Generic()
                };
                return options;
            }

            throw new ParserException($"'[' or '(' expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private InstanceOptions2 InstanceOptions2()
        {
            var row = GetTokenRow();
            var col = GetTokenColumn();

            if (IsUnaryExpression())
            {
                var instanceOptionsExpressionList =
                    new NewInstanceOptionsExpressionList
                    {
                        Row = row,
                        Col = col,
                        ExpressionList = ExpressionList()
                    };
                if (!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                instanceOptionsExpressionList.RankSpecifier = OptionalRankSpecifierList();
                instanceOptionsExpressionList.ArrayInitializer = OptionalArrayInitializer();

                return instanceOptionsExpressionList;
            }

            if (CheckTokenType(TokenType.Comma) || CheckTokenType(TokenType.SquareBracketClose))
            {
                var instanceOptionsCommaList = new NewInstanceOptionsCommaList
                {
                    Row = row,
                    Col = col,
                    CommaList = new List<int>{ OptionalCommaList() },
                };
                if (!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                instanceOptionsCommaList.RankSpecifier = OptionalRankSpecifierList();
                instanceOptionsCommaList.ArrayInitializer = ArrayInitializer();

                return instanceOptionsCommaList;
            }

            throw new ParserException($"Unary expression or comma token expected at row {GetTokenRow()} column {GetTokenColumn()}.");
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

        private CastOrParenthesizedExpression CastOrParenthesizedExpression()
        {
            if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            var row = GetTokenRow();
            var col = GetTokenColumn();

            NextToken();
            var castOrParenthesizedExpression = new CastOrParenthesizedExpression
            {
                Row = row,
                Col = col,
                ParenthesisExpression = Expression()
            };
            if(!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            //TODO BUG/Semantic: (5 * 9 / 3) - 7 => Toma el '- 7' como una nueva expresion negativa en lugar de tomar el '-' como operador binario.
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
