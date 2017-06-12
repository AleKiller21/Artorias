using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Expressions.Binary.Additive;
using SyntaxAnalyser.Nodes.Expressions.Binary.Assignment;
using SyntaxAnalyser.Nodes.Expressions.Binary.Equality;
using SyntaxAnalyser.Nodes.Expressions.Binary.IsAs;
using SyntaxAnalyser.Nodes.Expressions.Binary.Multiplicative;
using SyntaxAnalyser.Nodes.Expressions.Binary.Relational;
using SyntaxAnalyser.Nodes.Expressions.Binary.Shift;
using SyntaxAnalyser.Nodes.Expressions.Unary;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private bool IsAssignmentOperator()
        {
            return CheckTokenType(TokenType.OpAssignment) ||
                   CheckTokenType(TokenType.OpAddEqual) ||
                   CheckTokenType(TokenType.OpSubtractEqual) ||
                   CheckTokenType(TokenType.OpMultiplyEqual) ||
                   CheckTokenType(TokenType.OpDivideEqual) ||
                   CheckTokenType(TokenType.OpModuloEqual) ||
                   CheckTokenType(TokenType.OpAmpersandEqual) ||
                   CheckTokenType(TokenType.OpOrEqual) ||
                   CheckTokenType(TokenType.OpXorEqual) ||
                   CheckTokenType(TokenType.OpShiftLeftEqual) ||
                   CheckTokenType(TokenType.OpShiftRightEqual);
        }

        private bool IsExpressionRelationalOperator()
        {
            return CheckTokenType(TokenType.OpLessThan) ||
                   CheckTokenType(TokenType.OpGreaterThan) ||
                   CheckTokenType(TokenType.OpLessThanOrEqual) ||
                   CheckTokenType(TokenType.OpGreaterThanOrEqual);
        }

        private bool IsExpressionIsAsOperator()
        {
            return CheckTokenType(TokenType.OpIsType) || CheckTokenType(TokenType.OpAsType);
        }

        private bool IsExpressionEqualityOperator()
        {
            return CheckTokenType(TokenType.OpEqual) ||
                   CheckTokenType(TokenType.OpNotEqual);
        }

        private bool IsExpressionShiftOperator()
        {
            return CheckTokenType(TokenType.OpShiftLeft) ||
                   CheckTokenType(TokenType.OpShiftRight);
        }

        private bool IsAdditiveOperator()
        {
            return CheckTokenType(TokenType.OpAddition) ||
                   CheckTokenType(TokenType.OpSubtract);
        }

        private bool IsMultiplicativeOperator()
        {
            return CheckTokenType(TokenType.OpMultiply) ||
                   CheckTokenType(TokenType.OpDivision) ||
                   CheckTokenType(TokenType.OpModulo);
        }

        private bool IsExpressionUnaryOperator()
        {
            return CheckTokenType(TokenType.OpAddition) ||
                   CheckTokenType(TokenType.OpSubtract) ||
                   CheckTokenType(TokenType.OpLogicalNegation) ||
                   CheckTokenType(TokenType.OpBitwiseNegation) ||
                   CheckTokenType(TokenType.OpMultiply);
        }

        private bool IsIncrementDecrementOperator()
        {
            return CheckTokenType(TokenType.OpIncrement) ||
                   CheckTokenType(TokenType.OpDecrement);
        }

        private AssignmentOperator AssignmentOperator()
        {
            if (IsAssignmentOperator())
            {
                AssignmentOperator Operator;
                if(CheckTokenType(TokenType.OpAddEqual)) Operator = new PlusEqualOperator{Row = GetTokenRow(), Col = GetTokenColumn()};
                else if(CheckTokenType(TokenType.OpSubtractEqual)) Operator = new MinusEqualOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else if (CheckTokenType(TokenType.OpAssignment)) Operator = new AssignOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else if (CheckTokenType(TokenType.OpMultiplyEqual)) Operator = new MultiplicationEqualOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else if (CheckTokenType(TokenType.OpDivideEqual)) Operator = new DivideEqualOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else if (CheckTokenType(TokenType.OpModuloEqual)) Operator = new ModuloEqualOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else if (CheckTokenType(TokenType.OpAmpersandEqual)) Operator = new AndEqualOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else if (CheckTokenType(TokenType.OpOrEqual)) Operator = new OrEqualOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else if (CheckTokenType(TokenType.OpXorEqual)) Operator = new ExclusiveOrEqualOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else if (CheckTokenType(TokenType.OpShiftLeftEqual)) Operator = new LeftShiftEqualOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else Operator = new RightShiftEqualOperator { Row = GetTokenRow(), Col = GetTokenColumn() };

                NextToken();
                return Operator;
            }

            throw new AssignmentOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private RelationalOperator ExpressionRelationalOperator()
        {
            if (IsExpressionRelationalOperator())
            {
                RelationalOperator Operator;

                if(CheckTokenType(TokenType.OpLessThan)) Operator = new LessThanOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else if(CheckTokenType(TokenType.OpGreaterThan)) Operator = new GreaterThanOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else if (CheckTokenType(TokenType.OpLessThanOrEqual)) Operator = new LessThanOrEqualOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else Operator = new GreaterThanOrEqualOperator { Row = GetTokenRow(), Col = GetTokenColumn() };

                NextToken();
                return Operator;
            }

            throw new ExpressionRelationalOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private IsAsOperator ExpressionIsAsOperator()
        {
            IsAsOperator Operator;

            if (CheckTokenType(TokenType.OpIsType)) Operator = new IsOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
            else Operator = new AsOperator { Row = GetTokenRow(), Col = GetTokenColumn() };

            NextToken();
            return Operator;
        }

        private EqualityOperator ExpressionEqualityOperator()
        {
            if (IsExpressionEqualityOperator())
            {
                EqualityOperator Operator;

                if (CheckTokenType(TokenType.OpEqual)) Operator = new EqualOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else Operator = new NotEqualOperator { Row = GetTokenRow(), Col = GetTokenColumn() };

                NextToken();
                return Operator;
            }

            throw new ExpressionEqualityOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private ShiftOperator ExpressionShiftOperator()
        {
            if (IsExpressionShiftOperator())
            {
                ShiftOperator Operator;

                if(CheckTokenType(TokenType.OpShiftLeft)) Operator = new LeftShiftOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else Operator = new RightShiftOperator { Row = GetTokenRow(), Col = GetTokenColumn() };

                NextToken();
                return Operator;
            }

            throw new ExpressionShiftOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private AdditiveOperator AdditiveOperator()
        {
            if (IsAdditiveOperator())
            {
                AdditiveOperator Operator;

                if(CheckTokenType(TokenType.OpAddition)) Operator = new SumOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else Operator = new MinusOperator { Row = GetTokenRow(), Col = GetTokenColumn() };

                NextToken();
                return Operator;
            }

            throw new AdditiveOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private MultiplicativeOperator MultiplicativeOperator()
        {
            if (IsMultiplicativeOperator())
            {
                MultiplicativeOperator Operator;
                if(CheckTokenType(TokenType.OpMultiply)) Operator = new MultiplicationOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else if(CheckTokenType(TokenType.OpDivision)) Operator = new DivisionOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else Operator = new ModuloOperator { Row = GetTokenRow(), Col = GetTokenColumn() };

                NextToken();
                return Operator;
            }

            throw new MultiplicativeOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private UnaryOperator ExpressionUnaryOperator()
        {
            if (IsExpressionUnaryOperator())
            {
                UnaryOperator Operator;

                if(CheckTokenType(TokenType.OpAddition)) Operator = new PositiveOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else if(CheckTokenType(TokenType.OpSubtract)) Operator = new NegativeOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else if (CheckTokenType(TokenType.OpLogicalNegation)) Operator = new NegationOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else if (CheckTokenType(TokenType.OpBitwiseNegation)) Operator = new BitwiseNegationOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else Operator = new PointerOperator { Row = GetTokenRow(), Col = GetTokenColumn() };

                NextToken();
                return Operator;
            }

            throw new ExpressionUnaryOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private UnaryOperator ExpressionUnaryOperatorOrIncrementDecrement()
        {
            if (IsExpressionUnaryOperator()) return ExpressionUnaryOperator();
            if (IsIncrementDecrementOperator()) return IncrementDecrement();

            throw new ParserException($"Unary operator or increment-decrement operator expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private UnaryOperator IncrementDecrement()
        {
            if (IsIncrementDecrementOperator())
            {
                UnaryOperator Operator;
                if(CheckTokenType(TokenType.OpIncrement)) Operator = new PostIncrementOperator { Row = GetTokenRow(), Col = GetTokenColumn() };
                else Operator = new PostDecrementOperator { Row = GetTokenRow(), Col = GetTokenColumn() };

                NextToken();
                return Operator;
            }

            throw new IncrementDecrementOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }
    }
}
