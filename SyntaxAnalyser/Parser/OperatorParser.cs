using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Expressions.Binary.Additive;
using SyntaxAnalyser.Nodes.Expressions.Binary.Assignment;
using SyntaxAnalyser.Nodes.Expressions.Binary.Equality;
using SyntaxAnalyser.Nodes.Expressions.Binary.Multiplicative;
using SyntaxAnalyser.Nodes.Expressions.Binary.Relational;
using SyntaxAnalyser.Nodes.Expressions.Binary.Shift;

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
                   CheckTokenType(TokenType.OpGreaterThanOrEqual) ||
                   CheckTokenType(TokenType.OpIsType) || CheckTokenType(TokenType.OpAsType);
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
                if(CheckTokenType(TokenType.OpAddEqual)) Operator = new PlusEqualOperator();
                else if(CheckTokenType(TokenType.OpSubtractEqual)) Operator = new MinusEqualOperator();
                else if (CheckTokenType(TokenType.OpAssignment)) Operator = new AssignOperator();
                else if (CheckTokenType(TokenType.OpMultiplyEqual)) Operator = new MultiplicationEqualOperator();
                else if (CheckTokenType(TokenType.OpDivideEqual)) Operator = new DivideEqualOperator();
                else if (CheckTokenType(TokenType.OpModuloEqual)) Operator = new ModuloEqualOperator();
                else if (CheckTokenType(TokenType.OpAmpersandEqual)) Operator = new AndEqualOperator();
                else if (CheckTokenType(TokenType.OpOrEqual)) Operator = new OrEqualOperator();
                else if (CheckTokenType(TokenType.OpXorEqual)) Operator = new ExclusiveOrEqualOperator();
                else if (CheckTokenType(TokenType.OpShiftLeftEqual)) Operator = new LeftShiftEqualOperator();
                else Operator = new RightShiftEqualOperator();

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

                if(CheckTokenType(TokenType.OpLessThan)) Operator = new LessThanOperator();
                else if(CheckTokenType(TokenType.OpGreaterThan)) Operator = new GreaterThanOperator();
                else if (CheckTokenType(TokenType.OpLessThanOrEqual)) Operator = new LessThanOrEqualOperator();
                else if (CheckTokenType(TokenType.OpGreaterThanOrEqual)) Operator = new GreaterThanOrEqualOperator();
                else if (CheckTokenType(TokenType.OpIsType)) Operator = new IsOperator();
                else Operator = new AsOperator();

                NextToken();
                return Operator;
            }

            throw new ExpressionRelationalOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private EqualityOperator ExpressionEqualityOperator()
        {
            if (IsExpressionEqualityOperator())
            {
                EqualityOperator Operator;

                if (CheckTokenType(TokenType.OpEqual)) Operator = new EqualOperator();
                else Operator = new NotEqualOperator();

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

                if(CheckTokenType(TokenType.OpShiftLeft)) Operator = new LeftShiftOperator();
                else Operator = new RightShiftOperator();

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

                if(CheckTokenType(TokenType.OpAddition)) Operator = new SumOperator();
                else Operator = new MinusOperator();

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
                if(CheckTokenType(TokenType.OpMultiply)) Operator = new MultiplicationOperator();
                else if(CheckTokenType(TokenType.OpDivision)) Operator = new DivisionOperator();
                else Operator = new ModuloOperator();

                NextToken();
                return Operator;
            }

            throw new MultiplicativeOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private void ExpressionUnaryOperator()
        {
            if (IsExpressionUnaryOperator()) NextToken();
            else throw new ExpressionUnaryOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private void ExpressionUnaryOperatorOrIncrementDecrement()
        {
            if (IsExpressionUnaryOperator()) ExpressionUnaryOperator();
            else if (IsIncrementDecrementOperator()) IncrementDecrement();
            else throw new ParserException($"Unary operator or increment-decrement operator expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void IncrementDecrement()
        {
            if (IsIncrementDecrementOperator()) NextToken();
            else throw new IncrementDecrementOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }
    }
}
