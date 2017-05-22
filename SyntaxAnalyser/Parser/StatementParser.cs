using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
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

        private bool IsJumpStatement()
        {
            return CheckTokenType(TokenType.RwBreak) || CheckTokenType(TokenType.RwContinue) ||
                   CheckTokenType(TokenType.RwReturn);
        }

        private void Statement()
        {
            if (IsBuiltInType() || CheckTokenType(TokenType.RwOrIdVar) ||
                CheckTokenType(TokenType.Id) ||
                IsExpressionUnaryOperator() || IsIncrementDecrementOperator() ||
                IsPrimaryExpression())
            {
                StatementOptions();
                if(!CheckTokenType(TokenType.EndStatement))
                    throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }

            else if(IsEmptyBlock() || IsSelectionStatement() || IsIterationStatement() || IsJumpStatement())
                EmbeddedStatement();

            else throw new ParserException($"Begin of statement expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void StatementOptions()
        {
            if (IsBuiltInType() || CheckTokenType(TokenType.RwOrIdVar)) StatementLocalVariableDeclaration();
            else if (CheckTokenType(TokenType.Id))
            {
                NextToken();
                StatementIdentifierOptions();
            }
            else if (IsExpressionUnaryOperator() || IsIncrementDecrementOperator()) StatementStatementExpression();
            else if (IsPrimaryExpression())
            {
                PrimaryExpression();
                StatementExpressionPrimaryExpressionOptions();
            }
            else throw new ParserException($"Primary expression, expression unary operator, ++ or --, identifer, builtin type or var keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void StatementIdentifierOptions()
        {
            IdentifierAttribute();
            StatementIdentifierOptions2();
        }

        private void StatementIdentifierOptions2()
        {
            //TODO factorizar ambas producciones empiezan con '['
            if (CheckTokenType(TokenType.SquareBracketOpen))
            {
                OptionalRankSpecifierList();
                VariableDeclaratorList();
            }
            else if (IsPrimaryExpressionPrime())
            {
                PrimaryExpressionPrime();
                StatementExpressionPrimaryExpressionOptions();
            }

            else throw new ParserException($"'[', '(', '[', '.', '++', or '--' tokens expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void StatementStatementExpression()
        {
            if (IsExpressionUnaryOperator())
            {
                ExpressionUnaryOperator();
                UnaryExpression();
                AssignmentOperator();
                Expression();
            }

            else if (IsIncrementDecrementOperator())
            {
                IncrementDecrement();
                PrimaryExpression();
            }

            else throw new ParserException($"Unary operator or '++', '--' expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void StatementLocalVariableDeclaration()
        {
            if (IsBuiltInType())
            {
                BuiltInType();
                OptionalRankSpecifierList();
                VariableDeclaratorList();
            }

            else if (CheckTokenType(TokenType.RwOrIdVar))
            {
                NextToken();
                VariableDeclaratorList();
            }

            else throw new ParserException($"Builtin type or var keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void StatementList()
        {
            Statement();
            OptionalStatementList();
        }

        private void OptionalStatementList()
        {
            if (IsBuiltInType() || CheckTokenType(TokenType.RwOrIdVar) ||
                CheckTokenType(TokenType.Id) ||
                IsExpressionUnaryOperator() || IsIncrementDecrementOperator() ||
                IsPrimaryExpression() || IsEmptyBlock() || IsSelectionStatement() || IsIterationStatement() ||
                IsJumpStatement())
            {
                StatementList();
            }
            else
            {
                //Epsilon
            }
        }

        private void EmbeddedStatement()
        {
            //BUG ';' al entrar a jump statement
            if(IsEmptyBlock()) MaybeEmptyBlock();
            else if (IsSelectionStatement()) SelectionStatement();
            else if(IsIterationStatement()) IterationStatement();
            else if (IsJumpStatement())
            {
                JumpStatement();
                if(!CheckTokenType(TokenType.EndStatement))
                    throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }
            else throw new ParserException($"Curly brace opened, ';', selection statement, iteration statement or jump statement expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void EmbeddedStatementOrStatementExpression()
        {
            //BUG posiblemente otro bug con ';'. Examinar mejor
            if(IsEmptyBlock() || IsSelectionStatement() || IsIterationStatement() || IsJumpStatement()) EmbeddedStatement();
            else if (IsPrimaryExpression() || CheckTokenType(TokenType.Id) ||
                     IsExpressionUnaryOperator() || IsIncrementDecrementOperator())
            {
                StatementExpression();
                if(!CheckTokenType(TokenType.EndStatement))
                    throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }
            else throw new ParserException($"EmbeddedStatementOrStatementExpression error at row {GetTokenRow()} column {GetTokenColumn()}.");
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
            EmbeddedStatementOrStatementExpression();
        }

        private void ForStatement()
        {
            if (!CheckTokenType(TokenType.RwFor))
                throw new ParserException($"'for' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            OptionalForInitializer();
            if (!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();
            OptionalExpression();
            if (!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();
            OptionalStatementExpressionList();
            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            EmbeddedStatementOrStatementExpression();
        }

        private void DoStatement()
        {
            if (!CheckTokenType(TokenType.RwDo))
                throw new ParserException($"'do' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            EmbeddedStatementOrStatementExpression();
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
            EmbeddedStatementOrStatementExpression();
        }

        private void JumpStatement()
        {
            if (CheckTokenType(TokenType.RwBreak) || CheckTokenType(TokenType.RwContinue)) NextToken();
            else if (CheckTokenType(TokenType.RwReturn))
            {
                NextToken();
                OptionalExpression();
                if (!CheckTokenType(TokenType.EndStatement))
                    throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }

            else throw new ParserException($"'break', 'continue', or 'return' statements expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private void OptionalExpression()
        {
            //TODO Revisar bien todo el camino que debe tomar para saber si puede irse por Expression
            if (IsExpressionUnaryOperator() || IsIncrementDecrementOperator() ||
                IsPrimaryExpression() || CheckTokenType(TokenType.Id))
            {
                Expression();
            }

            else
            {
                //Epsilon
            }
        }

        private void OptionalForInitializer()
        {
            //BUG Ambiguedad con identifier en local-variable-declaration y statement-expression-list
            if (IsType() || CheckTokenType(TokenType.RwOrIdVar)) LocalVariableDeclaration();

            else if (IsPrimaryExpression() || CheckTokenType(TokenType.Id) || IsExpressionUnaryOperator() || IsIncrementDecrementOperator())
            {
                StatementExpressionList();
            }
            else
            {
                //Epsilon
            }
        }

        private void SelectionStatement()
        {
            if (CheckTokenType(TokenType.RwIf)) IfStatement();
            else if (CheckTokenType(TokenType.RwSwitch)) SwitchStatement();
            else throw new ParserException($"'if' or 'switch' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void SwitchStatement()
        {
            if(!CheckTokenType(TokenType.RwSwitch))
                throw new ParserException($"switch keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Expression();
            if(!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if(!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new MissingCurlyBraceOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            OptionalSwitchSectionList();
            if(!CheckTokenType(TokenType.CurlyBraceClose))
                throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }

        private void OptionalSwitchSectionList()
        {
            if (CheckTokenType(TokenType.RwCase) || CheckTokenType(TokenType.RwDefault))
            {
                SwitchLabelList();
                StatementList();
                OptionalSwitchSectionList();
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

        private void SwitchLabel()
        {
            if (CheckTokenType(TokenType.RwCase))
            {
                NextToken();
                Expression();
                if(!CheckTokenType(TokenType.Colon))
                    throw new ColonExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }

            else if (CheckTokenType(TokenType.RwDefault))
            {
                NextToken();
                if(!CheckTokenType(TokenType.Colon))
                    throw new ColonExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }

            else throw new ParserException($"'case' or 'default' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void IfStatement()
        {
            if(!CheckTokenType(TokenType.RwIf))
                throw new ParserException($"'if' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            Expression();
            if(!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            EmbeddedStatementOrStatementExpression();
            OptionalElsePart();
        }

        private void OptionalElsePart()
        {
            if (CheckTokenType(TokenType.RwElse)) ElsePart();
            else
            {
                //Epsilon
            }
        }

        private void ElsePart()
        {
            if(!CheckTokenType(TokenType.RwElse))
                throw new ParserException($"'else' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            EmbeddedStatementOrStatementExpression();
        }

        private void LocalVariableDeclaration()
        {
            TypeOrVar();
            VariableDeclaratorList();
        }

        private void StatementExpression()
        {
            if (IsPrimaryExpression() || CheckTokenType(TokenType.Id))
            {
                PrimaryExpressionOrIdentifier();
                StatementExpressionPrimaryExpressionOptions();
            }

            else if (IsExpressionUnaryOperator())
            {
                ExpressionUnaryOperator();
                UnaryExpression();
                AssignmentOperator();
                Expression();
            }

            else if (IsIncrementDecrementOperator())
            {
                IncrementDecrement();
                PrimaryExpression();
            }
            else throw new ParserException($"StatementExpression error at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void StatementExpressionPrimaryExpressionOptions()
        {
            if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                NextToken();
                ArgumentList();
                if(!CheckTokenType(TokenType.ParenthesisClose))
                    throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
            }

            else if(IsIncrementDecrementOperator()) IncrementDecrement();
            else if (IsAssignmentOperator())
            {
                AssignmentOperator();
                Expression();
            }
            else throw new ParserException($"'(', '++', '--', or assignment operator expected at row {GetTokenRow()} column {GetTokenColumn()}.");
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
                StatementExpression();
                StatementExpressionListPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private void OptionalStatementExpressionList()
        {
            if (IsPrimaryExpression() || CheckTokenType(TokenType.Id) || IsExpressionUnaryOperator() ||
                IsIncrementDecrementOperator())
            {
                StatementExpressionList();
            }

            else
            {
                //Epsilon
            }
        }
    }
}
