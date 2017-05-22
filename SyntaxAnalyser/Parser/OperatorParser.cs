using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;

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

        private bool Is_IsAsOperator()
        {
            return CheckTokenType(TokenType.OpIsType) ||
                   CheckTokenType(TokenType.OpAsType);
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

        private void AssignmentOperator()
        {
            if(IsAssignmentOperator()) NextToken();
            else throw new AssignmentOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private void ExpressionRelationalOperator()
        {
            if (IsExpressionRelationalOperator()) NextToken();
            else throw new ExpressionRelationalOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private void ExpressionEqualityOperator()
        {
            if (IsExpressionEqualityOperator()) NextToken();
            else throw new ExpressionEqualityOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private void ExpressionShiftOperator()
        {
            if(IsExpressionShiftOperator()) NextToken();
            else throw new ExpressionShiftOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private void AdditiveOperator()
        {
            if (IsAdditiveOperator()) NextToken();
            else throw new AdditiveOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private void MultiplicativeOperator()
        {
            if (IsMultiplicativeOperator()) NextToken();
            else throw new MultiplicativeOperatorExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private void IsAsOperator()
        {
            if (Is_IsAsOperator()) NextToken();
            else throw new IsAsOperatorExpectedException(GetTokenRow(), GetTokenColumn());
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
