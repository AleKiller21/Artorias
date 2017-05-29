using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Expressions.Binary;
using SyntaxAnalyser.Nodes.Expressions.Ternary;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private Expression ConditionalExpression()
        {
            return ConditionalExpressionPrime(NullCoalescingExpression());
        }

        private Expression ConditionalExpressionPrime(Expression leftOperand)
        {
            if (CheckTokenType(TokenType.OpConditional))
            {
                var conditionalOperator = new ConditionalOperator{LeftOperand = leftOperand};

                NextToken();
                conditionalOperator.FirstRightOperand = Expression();
                if(!CheckTokenType(TokenType.Colon))
                    throw new ColonExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                conditionalOperator.SecondRightOperand = Expression();

                return conditionalOperator;
            }

            return leftOperand;
        }

        private Expression NullCoalescingExpression()
        {
            return NullCoalescingExpressionPrime(ConditionalOrExpression());
        }

        private Expression NullCoalescingExpressionPrime(Expression leftOperand)
        {
            if (CheckTokenType(TokenType.OpNullCoalescing))
            {
                var expression = new NullCoalescingOperator{LeftOperand = leftOperand};
                NextToken();
                expression.RightOperand = ConditionalOrExpression();
                NullCoalescingExpressionPrime(expression);
            }

            return leftOperand;
        }

        private Expression ConditionalOrExpression()
        {
            return ConditionalOrExpressionPrime(ConditionalAndExpression());
        }

        private Expression ConditionalOrExpressionPrime(Expression leftOperand)
        {
            if (CheckTokenType(TokenType.OpConditionalOr))
            {
                NextToken();
                var expression = new ConditionalOrOperator
                {
                    LeftOperand = leftOperand,
                    RightOperand = ConditionalAndExpression()
                };

                return ConditionalOrExpressionPrime(expression);
            }
            else
            {
                return leftOperand;
            }
        }

        private Expression ConditionalAndExpression()
        {
            return ConditionalAndExpressionPrime(InclusiveOrExpression());
        }

        private Expression ConditionalAndExpressionPrime(Expression leftOperand)
        {
            if (CheckTokenType(TokenType.OpConditionalAnd))
            {
                NextToken();
                var expression = new ConditionalAndOperator
                {
                    LeftOperand = leftOperand,
                    RightOperand = InclusiveOrExpression()
                };
                
                return ConditionalAndExpressionPrime(expression);
            }
            else
            {
                return leftOperand;
            }
        }

        private Expression InclusiveOrExpression()
        {
            return InclusiveOrExpressionPrime(ExclusiveOrExpression());
        }

        private Expression InclusiveOrExpressionPrime(Expression leftOperand)
        {
            if (CheckTokenType(TokenType.OpLogicalOr))
            {
                NextToken();
                var expression = new InclusiveOrOperator()
                {
                    LeftOperand = leftOperand,
                    RightOperand = ExclusiveOrExpression()
                };

                return InclusiveOrExpressionPrime(expression);
            }
            else
            {
                return leftOperand;
            }
        }

        private Expression ExclusiveOrExpression()
        {
            return ExclusiveOrExpressionPrime(AndExpression());
        }

        private Expression ExclusiveOrExpressionPrime(Expression leftOperand)
        {
            if (CheckTokenType(TokenType.OpLogicalXor))
            {
                NextToken();
                var expression = new ExclusiveOrOperator
                {
                    LeftOperand = leftOperand,
                    RightOperand = AndExpression()
                };

                return ExclusiveOrExpressionPrime(expression);
            }

            return leftOperand;
        }

        private Expression AndExpression()
        {
            return AndExpressionPrime(EqualityExpression());
        }

        private Expression AndExpressionPrime(Expression leftOperand)
        {
            if (CheckTokenType(TokenType.OpLogicalAnd))
            {
                NextToken();
                var expression = new AndOperator
                {
                    LeftOperand = leftOperand,
                    RightOperand = EqualityExpression()
                };

                return AndExpressionPrime(expression);   
            }

            return leftOperand;
        }

        private Expression EqualityExpression()
        {
            return EqualityExpressionPrime(RelationalExpression());
        }

        private Expression EqualityExpressionPrime(Expression leftOperand)
        {
            if (IsExpressionEqualityOperator())
            {
                var expression = ExpressionEqualityOperator();
                expression.LeftOperand = leftOperand;
                expression.RightOperand = RelationalExpression();

                return EqualityExpressionPrime(expression);
            }
            else
            {
                return leftOperand;
            }
        }

        private Expression RelationalExpression()
        {
            return RelationalExpressionPrime(ShiftExpression());
        }

        private Expression RelationalExpressionPrime(Expression leftOperand)
        {
            if (IsExpressionRelationalOperator())
            {
                var expression = ExpressionRelationalOperator();
                expression.LeftOperand = leftOperand;
                expression.RightOperand = ShiftExpression();

                return RelationalExpressionPrime(expression);
            }

            return leftOperand;
        }

        private Expression ShiftExpression()
        {
            return ShiftExpressionPrime(AdditiveExpression());
        }

        private Expression ShiftExpressionPrime(Expression leftOperand)
        {
            if (IsExpressionShiftOperator())
            {
                var expression = ExpressionShiftOperator();
                expression.LeftOperand = leftOperand;
                expression.RightOperand = AdditiveExpression();

                return ShiftExpressionPrime(expression);
            }

            return leftOperand;
        }

        private Expression AdditiveExpression()
        {
            return AdditiveExpressionPrime(MultiplicativeExpression());
        }

        private Expression AdditiveExpressionPrime(Expression leftOperand)
        {
            if (IsAdditiveOperator())
            {
                var expression = AdditiveOperator();
                expression.LeftOperand = leftOperand;
                expression.RightOperand = MultiplicativeExpression();

                return AdditiveExpressionPrime(expression);
            }

            return leftOperand;
        }

        private Expression MultiplicativeExpression()
        {
            return MultiplicativeExpressionFactorized(UnaryExpression());
        }

        private Expression MultiplicativeExpressionFactorized(Expression leftOperand)
        {
            if (IsAssignmentOperator())
            {
                var expression = AssignmentOperator();
                expression.LeftOperand = leftOperand;
                expression.RightOperand = Expression();

                return MultiplicativeExpressionPrime(expression);
            }

            if(IsMultiplicativeOperator()) MultiplicativeExpressionPrime(leftOperand);

            return leftOperand;
        }

        private Expression MultiplicativeExpressionPrime(Expression leftOperand)
        {
            if (IsMultiplicativeOperator())
            {
                var expression = MultiplicativeOperator();
                expression.LeftOperand = leftOperand;
                expression.RightOperand = UnaryExpression();

                return MultiplicativeExpressionPrime(expression);
            }

            return leftOperand;
        }
    }
}
