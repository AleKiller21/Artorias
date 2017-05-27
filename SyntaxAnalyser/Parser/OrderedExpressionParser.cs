using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private void ConditionalExpression()
        {
            NullCoalescingExpression();
            ConditionalExpressionPrime();
        }

        private void ConditionalExpressionPrime()
        {
            if (CheckTokenType(TokenType.OpConditional))
            {
                NextToken();
                Expression();
                if(!CheckTokenType(TokenType.Colon))
                    throw new ColonExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                Expression();
            }
            else
            {
                //Epsilon
            }
        }

        private void NullCoalescingExpression()
        {
            ConditionalOrExpression();
            NullCoalescingExpressionPrime();
        }

        private void NullCoalescingExpressionPrime()
        {
            if (CheckTokenType(TokenType.OpNullCoalescing))
            {
                NextToken();
                ConditionalOrExpression();
                NullCoalescingExpressionPrime();
            }
            else
            {
                //Epsilon   
            }
        }

        private void ConditionalOrExpression()
        {
            ConditionalAndExpression();
            ConditionalOrExpressionPrime();
        }

        private void ConditionalOrExpressionPrime()
        {
            if (CheckTokenType(TokenType.OpConditionalOr))
            {
                NextToken();
                ConditionalAndExpression();
                ConditionalOrExpressionPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private void ConditionalAndExpression()
        {
            InclusiveOrExpression();
            ConditionalAndExpressionPrime();
        }

        private void ConditionalAndExpressionPrime()
        {
            if (CheckTokenType(TokenType.OpConditionalAnd))
            {
                NextToken();
                InclusiveOrExpression();
                ConditionalAndExpressionPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private void InclusiveOrExpression()
        {
            ExclusiveOrExpression();
            InclusiveOrExpressionPrime();
        }

        private void InclusiveOrExpressionPrime()
        {
            if (CheckTokenType(TokenType.OpLogicalOr))
            {
                NextToken();
                ExclusiveOrExpression();
                InclusiveOrExpressionPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private void ExclusiveOrExpression()
        {
            AndExpression();
            ExclusiveOrExpressionPrime();
        }

        private void ExclusiveOrExpressionPrime()
        {
            if (CheckTokenType(TokenType.OpLogicalXor))
            {
                NextToken();
                AndExpression();
                ExclusiveOrExpressionPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private void AndExpression()
        {
            EqualityExpression();
            AndExpressionPrime();
        }

        private void AndExpressionPrime()
        {
            if (CheckTokenType(TokenType.OpLogicalAnd))
            {
                NextToken();
                EqualityExpression();
                AndExpressionPrime();   
            }
            else
            {
                //Epsilon
            }
        }

        private void EqualityExpression()
        {
            RelationalExpression();
            EqualityExpressionPrime();
        }

        private void EqualityExpressionPrime()
        {
            if (IsExpressionEqualityOperator())
            {
                ExpressionEqualityOperator();
                RelationalExpression();
                EqualityExpressionPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private void RelationalExpression()
        {
            ShiftExpression();
            RelationalExpressionPrime();
        }

        private void RelationalExpressionPrime()
        {
            if (IsExpressionRelationalOperator())
            {
                ExpressionRelationalOperator();
                ShiftExpression();
                RelationalExpressionPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private void ShiftExpression()
        {
            AdditiveExpression();
            ShiftExpressionPrime();
        }

        private void ShiftExpressionPrime()
        {
            if (IsExpressionShiftOperator())
            {
                ExpressionShiftOperator();
                AdditiveExpression();
                ShiftExpressionPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private void AdditiveExpression()
        {
            MultiplicativeExpression();
            AdditiveExpressionPrime();
        }

        private void AdditiveExpressionPrime()
        {
            if (IsAdditiveOperator())
            {
                AdditiveOperator();
                MultiplicativeExpression();
                AdditiveExpressionPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private void MultiplicativeExpression()
        {
            UnaryExpression();
            MultiplicativeExpressionFactorized();
        }

        private void MultiplicativeExpressionFactorized()
        {
            if (IsAssignmentOperator())
            {
                AssignmentOperator();
                Expression();
                MultiplicativeExpressionPrime();
            }

            else if(IsMultiplicativeOperator()) MultiplicativeExpressionPrime();
            else
            {
                //Epsilon
            }
        }

        private void MultiplicativeExpressionPrime()
        {
            if (IsMultiplicativeOperator())
            {
                MultiplicativeOperator();
                UnaryExpression();
                MultiplicativeExpressionPrime();
            }
            else
            {
                //Epsilon
            }
        }
    }
}
