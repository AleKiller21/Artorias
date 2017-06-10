using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Statements;
using SyntaxAnalyser.Nodes.Statements.IterationStatements;
using SyntaxAnalyser.Nodes.Statements.JumpStatements;
using SyntaxAnalyser.Nodes.Statements.SelectionStatements;
using SyntaxAnalyser.Nodes.Statements.StatementExpressions;
using SyntaxAnalyser.Nodes.Statements.SwitchStatement;

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

        private JumpStatement JumpStatement()
        {
            var statement = JumpStatementPrime();
            if (!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();
            return statement;
        }

        private JumpStatement JumpStatementPrime()
        {
            if (CheckTokenType(TokenType.RwBreak))
            {
                var breakStatement = new BreakStatement();

                NextToken();
                return breakStatement;
            }
            if (CheckTokenType(TokenType.RwContinue))
            {
                var continueStatement = new ContinueStatement();

                NextToken();
                return continueStatement;
            }
            if (CheckTokenType(TokenType.RwReturn))
            {
                var returnStatement = new ReturnStatement();

                NextToken();
                returnStatement.ReturnExpression = OptionalExpression();
                return returnStatement;
            }

            throw new ParserException($"'break', 'continue', or 'return' statements expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private Expression OptionalExpression()
        {
            if (IsUnaryExpression())
            {
                return Expression();
            }

            return null;
        }

        private Statement Statement()
        {
            if (IsEmptyBlock()) MaybeEmptyBlock();//TODO
            else if (IsSelectionStatement()) return SelectionStatement();
            else if (IsIterationStatement()) return IterationStatement();
            else if (IsJumpStatement()) return JumpStatement();
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

        private IterationStatement IterationStatement()
        {
            if (CheckTokenType(TokenType.RwWhile)) return WhileStatement();
            if (CheckTokenType(TokenType.RwDo)) return DoStatement();
            if (CheckTokenType(TokenType.RwFor)) return ForStatement();
            if (CheckTokenType(TokenType.RwForEach)) return ForEachStatement();

            throw new IterationStatementExpectedException(GetTokenRow(), GetTokenColumn());
        }

        private ForEachStatement ForEachStatement()
        {
            if (!CheckTokenType(TokenType.RwForEach))
                throw new ParserException($"'foreach' token expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var forEachStatement = new ForEachStatement{ IteratorType = TypeOrVar() };
            if (!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            forEachStatement.IteratorIdentifier = _token.Lexeme;
            NextToken();
            if (!CheckTokenType(TokenType.RwIn))
                throw new InKeywordExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            forEachStatement.EnumerableExpression = Expression();
            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            forEachStatement.StatementBody = Statement();

            return forEachStatement;
        }

        private ForStatement ForStatement()
        {
            if (!CheckTokenType(TokenType.RwFor))
                throw new ParserException($"'for' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var forStatement = new ForStatement{Initializer = ForInitializer() };
            if (!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();
            forStatement.ConditionExpression = OptionalExpression();
            if (!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();
            forStatement.StatementExpressionList = ForStatementExpressionList();
            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            forStatement.StatementBody = Statement();

            return forStatement;
        }

        //Can declare variables here
        private List<StatementExpression> ForInitializer()
        {
            return IsStatementExpression() ? StatementExpressionList() : new List<StatementExpression>();
        }

        //Can't declare variables here
        private List<StatementExpression> ForStatementExpressionList()
        {
            return IsStatementExpression() ? StatementExpressionList() : new List<StatementExpression>();
        }

        private List<StatementExpression> StatementExpressionList()
        {
            var statementExpression = StatementExpression();
            var statementExpressionList = StatementExpressionListPrime();

            statementExpressionList.Insert(0, statementExpression);
            return statementExpressionList;
        }

        private List<StatementExpression> StatementExpressionListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                return StatementExpressionList();
            }
            
            return new List<StatementExpression>();
        }

        private DoStatement DoStatement()
        {
            if (!CheckTokenType(TokenType.RwDo))
                throw new ParserException($"'do' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            var doStatement = new DoStatement{StatementBody = Statement() };

            if (!CheckTokenType(TokenType.RwWhile))
                throw new ParserException($"'while' keyword expected at row {GetTokenRow()}, column {GetTokenColumn()}.");

            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            doStatement.ConditionExpression = Expression();

            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if (!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();
            return doStatement;
        }

        private WhileStatement WhileStatement()
        {
            if (!CheckTokenType(TokenType.RwWhile))
                throw new ParserException($"'while' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var whileStatement = new WhileStatement{ConditionExpression = Expression() };

            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            whileStatement.StatementBody = Statement();

            return whileStatement;
        }

        private SelectionStatement SelectionStatement()
        {
            if (CheckTokenType(TokenType.RwIf)) return IfStatement();
            if (CheckTokenType(TokenType.RwSwitch)) return SwitchStatement();
            else throw new ParserException($"'if' or 'switch' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private IfStatement IfStatement()
        {
            if (!CheckTokenType(TokenType.RwIf))
                throw new ParserException($"'if' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var ifStatement = new IfStatement{TestValue = Expression()};

            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            ifStatement.Statement = Statement();
            ifStatement.ElseStatement = OptionalElsePart();

            return ifStatement;
        }

        private Statement OptionalElsePart()
        {
            if (CheckTokenType(TokenType.RwElse))
            {
                NextToken();
                return Statement();
            }

            return null;
        }

        private SwitchStatement SwitchStatement()
        {
            if (!CheckTokenType(TokenType.RwSwitch))
                throw new ParserException($"switch keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");

            NextToken();
            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var switchStatement = new SwitchStatement{TestValue = Expression()};

            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if (!CheckTokenType(TokenType.CurlyBraceOpen))
                throw new MissingCurlyBraceOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            switchStatement.Sections = SwitchBody();

            if (!CheckTokenType(TokenType.CurlyBraceClose))
                throw new MissingCurlyBraceClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            return switchStatement;
        }

        private List<SwitchSection> SwitchBody()
        {
            if (CheckTokenType(TokenType.RwCase) || CheckTokenType(TokenType.RwDefault))
            {
                var section = SwitchSection();
                var sections = SwitchBody();

                sections.Insert(0, section);
                return sections;
            }

            return new List<SwitchSection>();
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

        private List<Statement> StatementList()
        {
            var statement = Statement();
            var statements = StatementListPrime();

            statements.Insert(0, statement);
            return statements;
        }

        private List<Statement> StatementListPrime()
        {
            if (IsStatementExpression() || IsEmptyBlock() || IsSelectionStatement() || IsIterationStatement() ||
                IsJumpStatement())
            {
                return StatementList();
            }
            
            return new List<Statement>();
        }

        private List<SwitchLabel> SwitchLabelList()
        {
            var label = SwitchLabel();
            var labels = SwitchLabelListPrime();

            labels.Insert(0, label);
            return labels;
        }

        private List<SwitchLabel> SwitchLabelListPrime()
        {
            if (CheckTokenType(TokenType.RwCase) || CheckTokenType(TokenType.RwDefault)) return SwitchLabelList();
            return new List<SwitchLabel>();
        }

        private SwitchSection SwitchSection()
        {
            return new SwitchSection
            {
                Labels = SwitchLabelList(),
                Statement = StatementList()
            };
        }

        private SwitchLabel SwitchLabel()
        {
            if (CheckTokenType(TokenType.RwCase))
            {
                var switchLabel = new SwitchLabel{Label = Label.Case};

                NextToken();
                switchLabel.Expression = Expression();
                if (!CheckTokenType(TokenType.Colon))
                    throw new ColonExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                return switchLabel;
            }

            if (CheckTokenType(TokenType.RwDefault))
            {
                var switchLabel = new SwitchLabel { Label = Label.Default };

                NextToken();
                if (!CheckTokenType(TokenType.Colon))
                    throw new ColonExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                return switchLabel;
            }

            throw new ParserException($"'case' or 'default' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }
    }
}
