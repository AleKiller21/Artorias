﻿using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private bool IsUnaryExpression()
        {
            return IsExpressionUnaryOperator() || IsIncrementDecrementOperator() ||
                   IsPrimaryExpression();
        }

        private bool IsEmptyBlock()
        {
            return (CheckTokenType(TokenType.CurlyBraceOpen) || CheckTokenType(TokenType.EndStatement));
        }

        private bool IsSelectionStatement()
        {
            return CheckTokenType(TokenType.RwIf) || CheckTokenType(TokenType.RwSwitch);
        }

        private bool IsIterationStatement()
        {
            return CheckTokenType(TokenType.RwWhile) || CheckTokenType(TokenType.RwDo) ||
                   CheckTokenType(TokenType.RwFor) || CheckTokenType(TokenType.RwForEach);
        }

        private bool IsStatementExpression()
        {
            return IsBuiltInType() || CheckTokenType(TokenType.RwOrIdVar) || CheckTokenType(TokenType.Id) ||
                   IsExpressionUnaryOperator() || IsIncrementDecrementOperator() ||
                   CheckTokenType(TokenType.RwThis) || CheckTokenType(TokenType.RwBase) ||
                   CheckTokenType(TokenType.ParenthesisOpen) || CheckTokenType(TokenType.RwNew);
        }

        private bool IsJumpStatement()
        {
            return CheckTokenType(TokenType.RwBreak) || CheckTokenType(TokenType.RwContinue) ||
                   CheckTokenType(TokenType.RwReturn);
        }

        private void JumpStatement()
        {
            JumpStatementPrime();
            if (!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }

        private void JumpStatementPrime()
        {
            if (CheckTokenType(TokenType.RwBreak) || CheckTokenType(TokenType.RwContinue)) NextToken();
            else if (CheckTokenType(TokenType.RwReturn))
            {
                NextToken();
                OptionalExpression();
            }

            else throw new ParserException($"'break', 'continue', or 'return' statements expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private Expression OptionalExpression()
        {
            if (IsUnaryExpression())
            {
                return Expression();
            }

            return null;
        }

        private void Statement()
        {
            if (IsEmptyBlock()) MaybeEmptyBlock();
            else if (IsSelectionStatement()) SelectionStatement();
            else if (IsIterationStatement()) IterationStatement();
            else if (IsJumpStatement()) JumpStatement();
            else if (IsStatementExpression())
            {
                StatementExpression();
                if(!CheckTokenType(TokenType.EndStatement))
                    throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }

            else throw new ParserException($"Begin of statement expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void StatementExpression()
        {
            if (CheckTokenType(TokenType.RwThis)) ThisStatementExpression();
            else if (CheckTokenType(TokenType.RwBase)) BaseStatementExpression();
            else if (CheckTokenType(TokenType.Id)) QualifiedIdentifierStatementExpression();
            else if (IsIncrementDecrementOperator()) IncrementDecrementStatementExpression();
            else if (CheckTokenType(TokenType.RwNew)) NewObjectStatementExpression();
            else if (CheckTokenType(TokenType.ParenthesisOpen)) ParenthesizedStatementExpression();
            else if (IsBuiltInType() || CheckTokenType(TokenType.RwOrIdVar)) BuiltInDeclaration();

            else throw new ParserException($"Primary expression, expression unary operator, ++ or --, identifer, builtin type or var keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void ParenthesizedStatementExpression()
        {
            if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Expression();

            if(!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            OptionalExpression();
            ArrayAccess();

            if(!CheckTokenType(TokenType.MemberAccess))
                throw new MemberAccessExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            QualifiedIdentifier();

            if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            ArgumentList();
            if(!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            CallAccess();
        }

        private void BaseStatementExpression()
        {
            if(!CheckTokenType(TokenType.RwBase))
                throw new BaseKeywordExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if (!CheckTokenType(TokenType.MemberAccess))
                throw new MemberAccessExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            ArgumentList();
            if(!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            CallAccess();
        }

        private void BuiltInDeclaration()
        {
            BuiltInDeclarationPrime();
            OptionalRankSpecifierList();
            VariableDeclaratorList();
        }

        private void BuiltInDeclarationPrime()
        {
            if(IsBuiltInType()) BuiltInType();
            else if(CheckTokenType(TokenType.RwOrIdVar)) NextToken();
            else throw new ParserException($"Builtin type or 'var' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void NewObjectStatementExpression()
        {
            if (!CheckTokenType(TokenType.RwNew))
                throw new NewKeywordExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Type();
            if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            ArgumentList();
            if(!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            CallAccess();
        }

        private void CallAccess()
        {
            if (!CheckTokenType(TokenType.MemberAccess))
            {
                NextToken();
                QualifiedIdentifier();
                if (!CheckTokenType(TokenType.ParenthesisOpen))
                    throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

                NextToken();
                ArgumentList();

                if (!CheckTokenType(TokenType.ParenthesisClose))
                    throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                CallAccess();
            }
            else
            {
                //Epsilon
            }
        }

        private void IncrementDecrementStatementExpression()
        {
            IncrementDecrement();
            IncrementDecrementStatementExpressionPrime();
        }

        private void IncrementDecrementStatementExpressionPrime()
        {
            if (CheckTokenType(TokenType.RwThis))
            {
                NextToken();
                if(!CheckTokenType(TokenType.MemberAccess))
                    throw new MemberAccessExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                QualifiedIdentifier();
                ArrayAccess();
            }
            else if (CheckTokenType(TokenType.Id))
            {
                QualifiedIdentifier();
                ArrayAccess();
            }
            else throw new ParserException($"'this' keyword or id token expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void ThisStatementExpression()
        {
            if (!CheckTokenType(TokenType.RwThis))
                throw new ThisKeywordExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if(!CheckTokenType(TokenType.MemberAccess))
                throw new MemberAccessExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            QualifiedIdentifierStatementExpression();
        }

        private void QualifiedIdentifierStatementExpression()
        {
            QualifiedIdentifier();
            QualifiedIdentifierStatementExpressionPrime();
        }

        private void QualifiedIdentifierStatementExpressionPrime()
        {
            if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                NextToken();
                ArgumentList();
                if (!CheckTokenType(TokenType.ParenthesisClose))
                    throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                CallAccess();
            }
            else if (IsIncrementDecrementOperator()) IncrementDecrement();
            else if (CheckTokenType(TokenType.Id)) VariableDeclaratorList();
            else if (CheckTokenType(TokenType.SquareBracketOpen))
            {
                NextToken();
                QualifiedIdentifierStatementExpressionPrimePrime();
            }
            else if (IsAssignmentOperator())
            {
                AssignmentOperator();
                Expression();
            }

            else throw new ParserException($"'[', assignment operator, increment/decrement operator, '(', or identifier tokens expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void QualifiedIdentifierStatementExpressionPrimePrime()
        {
            if (CheckTokenType(TokenType.Comma) || CheckTokenType(TokenType.SquareBracketClose))
            {
                OptionalCommaList();
                if (!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                OptionalRankSpecifierList();
                VariableDeclaratorList();
            }
            else if (IsUnaryExpression())
            {
                ExpressionList();
                if (!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                ArrayAccess();
                QualifiedIdentifierStatementExpressionPrimePrimePrime();
            }
            else throw new ParserException($"Unary expression or ',' expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void QualifiedIdentifierStatementExpressionPrimePrimePrime()
        {
            //Array access increment-decrement
            if (IsIncrementDecrementOperator()) IncrementDecrement();

            //Array access assignment
            else if (IsAssignmentOperator())
            {
                AssignmentOperator();
                Expression();
            }

            else throw new ParserException($"increment/decrement or assignment operator expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void ArrayAccess()
        {
            if (CheckTokenType(TokenType.SquareBracketOpen))
            {
                NextToken();
                ExpressionList();
                if(!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                ArrayAccess();
            }

            else
            {
                //Epsilon
            }
        }

        private void IterationStatement()
        {
            if (CheckTokenType(TokenType.RwWhile)) WhileStatement();
            else if (CheckTokenType(TokenType.RwDo)) DoStatement();
            else if (CheckTokenType(TokenType.RwFor)) ForStatement();
            else if (CheckTokenType(TokenType.RwForEach)) ForEachStatement();
            else throw new IterationStatementExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private void ForEachStatement()
        {
            if (!CheckTokenType(TokenType.RwForEach))
                throw new ParserException($"'foreach' token expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            TypeOrVar();
            if (!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if (!CheckTokenType(TokenType.RwIn))
                throw new InKeywordExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Expression();
            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Statement();
        }

        private void ForStatement()
        {
            if (!CheckTokenType(TokenType.RwFor))
                throw new ParserException($"'for' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            ForInitializer();
            if (!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();
            OptionalExpression();
            if (!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();
            ForStatementExpressionList();
            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Statement();
        }

        //Can declare variables here
        private void ForInitializer()
        {
            if (IsStatementExpression()) StatementExpressionList();
            else
            {
                //Epsilon
            }
        }

        //Can't declare variables here
        private void ForStatementExpressionList()
        {
            if(IsStatementExpression()) StatementExpressionList();
            else
            {
                //Epsilon
            }
        }

        private void StatementExpressionList()
        {
            StatementExpression();
            StatementExpressionListPrime();
        }

        private void StatementExpressionListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                StatementExpressionList();
            }
            else
            {
                //Epsilon
            }
        }

        private void DoStatement()
        {
            if (!CheckTokenType(TokenType.RwDo))
                throw new ParserException($"'do' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            Statement();
            if (!CheckTokenType(TokenType.RwWhile))
                throw new ParserException($"'while' keyword expected at row {GetTokenRow()}, column {GetTokenColumn()}.");

            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Expression();
            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if (!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }

        private void WhileStatement()
        {
            if (!CheckTokenType(TokenType.RwWhile))
                throw new ParserException($"'while' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Expression();
            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Statement();
        }

        private void SelectionStatement()
        {
            if (CheckTokenType(TokenType.RwIf)) IfStatement();
            else if (CheckTokenType(TokenType.RwSwitch)) SwitchStatement();
            else throw new ParserException($"'if' or 'switch' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void IfStatement()
        {
            if (!CheckTokenType(TokenType.RwIf))
                throw new ParserException($"'if' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Expression();
            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Statement();
            OptionalElsePart();
        }

        private void OptionalElsePart()
        {
            if (CheckTokenType(TokenType.RwElse))
            {
                NextToken();
                Statement();
            }
            else
            {
                //Epsilon
            }
        }

        private void SwitchStatement()
        {
            if (!CheckTokenType(TokenType.RwSwitch))
                throw new ParserException($"switch keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Expression();
            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if (!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new MissingCurlyBraceOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            SwitchBody();
            if (!CheckTokenType(TokenType.CurlyBraceClose))
                throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }

        private void SwitchBody()
        {
            if (CheckTokenType(TokenType.RwCase) || CheckTokenType(TokenType.RwDefault))
            {
                SwitchSection();
                SwitchBody();
            }
            else
            {
                //Epsilon
            }
        }

        private void OptionalStatementList()
        {
            if (IsStatementExpression() || IsEmptyBlock() || IsSelectionStatement() || IsIterationStatement() ||
                IsJumpStatement())
            {
                StatementList();
            }
            else
            {
                //Epsilon
            }
        }

        private void StatementList()
        {
            Statement();
            StatementListPrime();
        }

        private void StatementListPrime()
        {
            if (IsStatementExpression() || IsEmptyBlock() || IsSelectionStatement() || IsIterationStatement() ||
                IsJumpStatement())
            {
                StatementList();
            }
            else
            {
                //Epsilon
            }
        }

        private void SwitchLabelList()
        {
            SwitchLabel();
            SwitchLabelListPrime();
        }

        private void SwitchLabelListPrime()
        {
            if (CheckTokenType(TokenType.RwCase) || CheckTokenType(TokenType.RwDefault)) SwitchLabelList();
            else
            {
                //Epsilon
            }
        }

        private void SwitchSection()
        {
            SwitchLabelList();
            StatementList();
        }

        private void SwitchLabel()
        {
            if (CheckTokenType(TokenType.RwCase))
            {
                NextToken();
                Expression();
                if (!CheckTokenType(TokenType.Colon))
                    throw new ColonExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }

            else if (CheckTokenType(TokenType.RwDefault))
            {
                NextToken();
                if (!CheckTokenType(TokenType.Colon))
                    throw new ColonExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }

            else throw new ParserException($"'case' or 'default' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }
    }
}
