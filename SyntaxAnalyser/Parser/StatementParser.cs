using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private bool IsUnaryExpression()
        {
            return IsExpressionUnaryOperator() || IsIncrementDecrementOperator() ||
                   IsPrimaryExpression() || CheckTokenType(TokenType.Id);
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

        private bool IsJumpStatement()
        {
            return CheckTokenType(TokenType.RwBreak) || CheckTokenType(TokenType.RwContinue) ||
                   CheckTokenType(TokenType.RwReturn);
        }

        private void Statement()
        {
            if (IsStatementOptions())
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

        private bool IsStatementOptions()
        {
            return IsBuiltInType() || CheckTokenType(TokenType.RwOrIdVar) || CheckTokenType(TokenType.Id) ||
                   IsExpressionUnaryOperator() || IsIncrementDecrementOperator() ||
                   CheckTokenType(TokenType.RwThis) || CheckTokenType(TokenType.RwBase) ||
                   CheckTokenType(TokenType.ParenthesisOpen);
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
            else if (CheckTokenType(TokenType.RwThis))
            {
                NextToken();
                PrimaryExpressionPrime();
                VariableAssigner();
            }
            else if (CheckTokenType(TokenType.RwBase))
            {
                NextToken();
                PrimaryExpressionPrime();
                VariableAssigner();
            }
            else if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                CastOrParenthesizedExpression();
                PrimaryExpressionPrime();
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
            if (CheckTokenType(TokenType.SquareBracketOpen))
            {
                NextToken();
                StatementIdentifierOptions3();
            }
            else if (CheckTokenType(TokenType.OpLessThan))
            {
                Generic();
                VariableDeclaratorList();
            }
            else if (CheckTokenType(TokenType.Id)) VariableDeclaratorList();
            else if (IsAssignmentOperator())
            {
                VariableAssigner();
                VariableDeclaratorListPrime();
            }
            else if (CheckTokenType(TokenType.MemberAccess))
            {
                IdentifierAttribute();
                PrimaryExpressionPrime();
                StatementExpressionPrimaryExpressionOptions();
            }
            else if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                NextToken();
                ArgumentList();
                if(!CheckTokenType(TokenType.ParenthesisClose))
                    throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                StatementIdentifierOptions5();
            }
            else if(IsIncrementDecrementOperator()) IncrementDecrement();

            else throw new ParserException($"'[', assignment operator, '.', '(', or identifier tokens expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }
        private void StatementIdentifierOptions3()
        {
            if (CheckTokenType(TokenType.Comma) || CheckTokenType(TokenType.SquareBracketClose))
            {
                OptionalCommaList();
                if(!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                OptionalRankSpecifierList();
                VariableDeclaratorList();
            }
            else if (IsUnaryExpression())
            {
                ExpressionList();
                if(!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                StatementIdentifierOptions4();
            }
            else throw new ParserException($"Unary expression or ',' expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void StatementIdentifierOptions4()
        {
            if (IsAssignmentOperator())
            {
                VariableAssigner();
                VariableDeclaratorListPrime();
            }
            else if(IsPrimaryExpressionPrime()) PrimaryExpressionPrime();
            else throw new ParserException($"Assignment operator, '(', '[', '.', '++', or '--' tokens expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private void StatementIdentifierOptions5()
        {
            if (IsPrimaryExpressionPrime())
            {
                PrimaryExpressionPrime();
                StatementExpressionPrimaryExpressionOptions();
            }
            else
            {
                //Epsilon
            }
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
                PrimaryExpressionOrIdentifier();
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
            if (IsStatementOptions() || IsEmptyBlock() || IsSelectionStatement() || IsIterationStatement() ||
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
            }

            else throw new ParserException($"'break', 'continue', or 'return' statements expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private void OptionalExpression()
        {
            if (IsUnaryExpression())
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
            if (IsStatementOptions())
            {
                StatementOptions();
                StatementOptionsList();
            }
            else
            {
                //Epsilon
            }
        }

        private void StatementOptionsList()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                StatementOptions();
                StatementOptionsList();
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
                PrimaryExpressionOrIdentifier();
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
            else
            {
                //Epsilon
            }
            //else throw new ParserException($"'(', '++', '--', or assignment operator expected at row {GetTokenRow()} column {GetTokenColumn()}.");
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
