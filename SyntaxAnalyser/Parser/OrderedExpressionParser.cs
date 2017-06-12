using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Expressions.Binary;
using SyntaxAnalyser.Nodes.Expressions.Binary.IsAs;
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
                var conditionalOperator = new ConditionalOperator
                {
                    Row = GetTokenRow(),
                    Col = GetTokenColumn(),
                    LeftOperand = leftOperand
                };

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
                var expression = new NullCoalescingOperator
                {
                    Row = GetTokenRow(),
                    Col = GetTokenColumn(),
                    LeftOperand = leftOperand
                };
                NextToken();
                expression.RightOperand = ConditionalOrExpression();
                return NullCoalescingExpressionPrime(expression);
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
                var row = GetTokenRow();
                var col = GetTokenColumn();
                NextToken();
                var expression = new ConditionalOrOperator
                {
                    Row = row,
                    Col = col,
                    LeftOperand = leftOperand,
                    RightOperand = ConditionalAndExpression()
                };

                return ConditionalOrExpressionPrime(expression);
            }

            return leftOperand;
        }

        private Expression ConditionalAndExpression()
        {
            return ConditionalAndExpressionPrime(InclusiveOrExpression());
        }

        private Expression ConditionalAndExpressionPrime(Expression leftOperand)
        {
            if (CheckTokenType(TokenType.OpConditionalAnd))
            {
                var row = GetTokenRow();
                var col = GetTokenColumn();
                NextToken();
                var expression = new ConditionalAndOperator
                {
                    Row = row,
                    Col = col,
                    LeftOperand = leftOperand,
                    RightOperand = InclusiveOrExpression()
                };
                
                return ConditionalAndExpressionPrime(expression);
            }
            return leftOperand;
        }

        private Expression InclusiveOrExpression()
        {
            return InclusiveOrExpressionPrime(ExclusiveOrExpression());
        }

        private Expression InclusiveOrExpressionPrime(Expression leftOperand)
        {
            if (CheckTokenType(TokenType.OpLogicalOr))
            {
                var row = GetTokenRow();
                var col = GetTokenColumn();
                NextToken();
                var expression = new InclusiveOrOperator()
                {
                    Row = row,
                    Col = col,
                    LeftOperand = leftOperand,
                    RightOperand = ExclusiveOrExpression()
                };

                return InclusiveOrExpressionPrime(expression);
            }
            return leftOperand;
        }

        private Expression ExclusiveOrExpression()
        {
            return ExclusiveOrExpressionPrime(AndExpression());
        }

        private Expression ExclusiveOrExpressionPrime(Expression leftOperand)
        {
            if (CheckTokenType(TokenType.OpLogicalXor))
            {
                var row = GetTokenRow();
                var col = GetTokenColumn();
                NextToken();
                var expression = new ExclusiveOrOperator
                {
                    Row = row,
                    Col = col,
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
                var row = GetTokenRow();
                var col = GetTokenColumn();
                NextToken();
                var expression = new AndOperator
                {
                    Row = row,
                    Col = col,
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
                var row = GetTokenRow();
                var col = GetTokenColumn();
                var expression = ExpressionEqualityOperator();
                expression.LeftOperand = leftOperand;
                expression.RightOperand = RelationalExpression();
                expression.Row = row;
                expression.Col = col;

                return EqualityExpressionPrime(expression);
            }
            return leftOperand;
        }

        private Expression RelationalExpression()
        {
            return RelationalExpressionPrime(ShiftExpression());
        }

        private Expression RelationalExpressionPrime(Expression leftOperand)
        {
            var row = GetTokenRow();
            var col = GetTokenColumn();

            if (IsExpressionRelationalOperator())
            {
                var expression = ExpressionRelationalOperator();
                expression.LeftOperand = leftOperand;
                expression.RightOperand = ShiftExpression();
                expression.Row = row;
                expression.Col = col;

                return RelationalExpressionPrime(expression);
            }
            if (IsExpressionIsAsOperator())
            {
                var expression = ExpressionIsAsOperator();
                expression.Row = row;
                expression.Col = col;
                expression.LeftOperand = leftOperand;
                expression.RightOperand = new IsAsTypeExpression
                {
                    Row = GetTokenRow(),
                    Col = GetTokenColumn(),
                    Type = Type()
                };

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
                var row = GetTokenRow();
                var col = GetTokenColumn();
                var expression = ExpressionShiftOperator();
                expression.LeftOperand = leftOperand;
                expression.RightOperand = AdditiveExpression();
                expression.Row = row;
                expression.Col = col;

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
                var row = GetTokenRow();
                var col = GetTokenColumn();
                var expression = AdditiveOperator();
                expression.LeftOperand = leftOperand;
                expression.RightOperand = MultiplicativeExpression();
                expression.Row = row;
                expression.Col = col;

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
                var row = GetTokenRow();
                var col = GetTokenColumn();
                var expression = AssignmentOperator();
                expression.LeftOperand = leftOperand;
                expression.RightOperand = Expression();
                expression.Row = row;
                expression.Col = col;

                return MultiplicativeExpressionPrime(expression);
            }

            if(IsMultiplicativeOperator()) return MultiplicativeExpressionPrime(leftOperand);

            return leftOperand;
        }

        private Expression MultiplicativeExpressionPrime(Expression leftOperand)
        {
            if (IsMultiplicativeOperator())
            {
                var row = GetTokenRow();
                var col = GetTokenColumn();
                var expression = MultiplicativeOperator();
                expression.LeftOperand = leftOperand;
                expression.RightOperand = UnaryExpression();
                expression.Row = row;
                expression.Col = col;

                return MultiplicativeExpressionPrime(expression);
            }

            return leftOperand;
        }
    }
}
