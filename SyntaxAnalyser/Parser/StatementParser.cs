using System.Collections.Generic;
using LexerAnalyser.Enums;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes.Expressions;
using SyntaxAnalyser.Nodes.Statements;
using SyntaxAnalyser.Nodes.Statements.IterationStatements;
using SyntaxAnalyser.Nodes.Statements.JumpStatements;
using SyntaxAnalyser.Nodes.Statements.SelectionStatements;
using SyntaxAnalyser.Nodes.Statements.StatementExpressions;
using SyntaxAnalyser.Nodes.Statements.StatementExpressions.ThisStatementExpressions;
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
            if (IsEmptyBlock()) return MaybeEmptyBlock();
            if (IsSelectionStatement()) return SelectionStatement();
            if (IsIterationStatement()) return IterationStatement();
            if (IsJumpStatement()) return JumpStatement();
            if (IsStatementExpression())
            {
                var statementExpression = StatementExpression();
                if(!CheckTokenType(TokenType.EndStatement))
                    throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

                NextToken();
                return statementExpression;
            }

            else throw new ParserException($"Begin of statement expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private StatementExpression StatementExpression()
        {
            if (CheckTokenType(TokenType.RwThis)) return ThisStatementExpression();
            if (CheckTokenType(TokenType.RwBase)) return BaseStatementExpression();
            if (CheckTokenType(TokenType.Id)) return QualifiedIdentifierStatementExpression();
            if (IsIncrementDecrementOperator()) return IncrementDecrementStatementExpression();
            if (CheckTokenType(TokenType.RwNew)) return NewObjectStatementExpression();
            if (CheckTokenType(TokenType.ParenthesisOpen)) return ParenthesizedStatementExpression();
            if (IsBuiltInType() || CheckTokenType(TokenType.RwOrIdVar)) return BuiltInDeclaration();

            throw new ParserException($"Primary expression, expression unary operator, ++ or --, identifer, builtin type or var keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private ParenthesizedStatementExpression ParenthesizedStatementExpression()
        {
            if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var parenthesizedExpression = new ParenthesizedStatementExpression{ParenthesisExpression = Expression() };

            if(!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            parenthesizedExpression.OptionalExpression = OptionalExpression();
            parenthesizedExpression.ArrayAccess = ArrayAccess();

            if (!CheckTokenType(TokenType.MemberAccess))
                throw new MemberAccessExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            parenthesizedExpression.MemberAccessQualifiedIdentifier = QualifiedIdentifier();

            if (!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            parenthesizedExpression.ArgumentList = ArgumentList();
            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            parenthesizedExpression.CallAccess = CallAccess();
            return parenthesizedExpression;
        }

        private BaseStatementExpression BaseStatementExpression()
        {
            if(!CheckTokenType(TokenType.RwBase))
                throw new BaseKeywordExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if (!CheckTokenType(TokenType.MemberAccess))
                throw new MemberAccessExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            var baseStatement = new BaseStatementExpression{MethodIdentifier = _token.Lexeme};
            NextToken();
            if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            baseStatement.ArgumentList = ArgumentList();
            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            baseStatement.CallAccess = CallAccess();
            return baseStatement;
        }

        private BuiltInDeclarationStatement BuiltInDeclaration()
        {
            var builtInDeclarationStatement = new BuiltInDeclarationStatement();
            BuiltInDeclarationPrime(builtInDeclarationStatement);
            builtInDeclarationStatement.OptionalRankSpecifierList = OptionalRankSpecifierList();
            builtInDeclarationStatement.VariableDeclaratorList = VariableDeclaratorList();

            return builtInDeclarationStatement;
        }

        private void BuiltInDeclarationPrime(BuiltInDeclarationStatement builtInDeclarationStatement)
        {
            if(IsBuiltInType()) builtInDeclarationStatement.BuiltInDataType = BuiltInType();
            else if (CheckTokenType(TokenType.RwOrIdVar))
            {
                builtInDeclarationStatement.IsVar = true;
                NextToken();
            }
            else throw new ParserException($"Builtin type or 'var' keyword expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private NewObjectStatementExpression NewObjectStatementExpression()
        {
            if (!CheckTokenType(TokenType.RwNew))
                throw new NewKeywordExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var newObjectExpression = new NewObjectStatementExpression{Type = Type() };
            if(!CheckTokenType(TokenType.ParenthesisOpen))
                throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

            NextToken();
            newObjectExpression.ArgumentList = ArgumentList();
            if (!CheckTokenType(TokenType.ParenthesisClose))
                throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            newObjectExpression.CallAccess = CallAccess();
            return newObjectExpression;
        }

        private CallAccess CallAccess()
        {
            if (!CheckTokenType(TokenType.MemberAccess))
            {
                NextToken();
                var callAccess = new CallAccess{MethodIdentifier = QualifiedIdentifier() };
                if (!CheckTokenType(TokenType.ParenthesisOpen))
                    throw new ParentesisOpenException(GetTokenRow(), GetTokenColumn());

                NextToken();
                callAccess.ArgumentList = ArgumentList();

                if (!CheckTokenType(TokenType.ParenthesisClose))
                    throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                callAccess.Call = CallAccess();
                return callAccess;
            }
            
            return new CallAccess();
        }

        private PreIncrementDecrementStatementExpression IncrementDecrementStatementExpression()
        {
            var operatorExpression = new PreIncrementDecrementStatementExpression{IncrementDecrement = IncrementDecrement() };
            IncrementDecrementStatementExpressionPrime(operatorExpression);

            return operatorExpression;
        }

        private void IncrementDecrementStatementExpressionPrime(PreIncrementDecrementStatementExpression operatorExpression)
        {
            if (CheckTokenType(TokenType.RwThis))
            {
                operatorExpression.UsingThisKeyword = true;
                NextToken();
                if(!CheckTokenType(TokenType.MemberAccess))
                    throw new MemberAccessExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                operatorExpression.Identifier = QualifiedIdentifier();
                operatorExpression.ArrayAccess = ArrayAccess();
            }
            else if (CheckTokenType(TokenType.Id))
            {
                operatorExpression.Identifier = QualifiedIdentifier();
                operatorExpression.ArrayAccess = ArrayAccess();
            }
            else throw new ParserException($"'this' keyword or id token expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private ThisStatementExpression ThisStatementExpression()
        {
            if (!CheckTokenType(TokenType.RwThis))
                throw new ThisKeywordExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if(!CheckTokenType(TokenType.MemberAccess))
                throw new MemberAccessExpectedException(GetTokenRow(), GetTokenColumn());

            NextToken();
            return new ThisStatementExpression{StatementExpression = QualifiedIdentifierStatementExpression() };
        }

        private QualifiedIdentifierStatementExpression QualifiedIdentifierStatementExpression()
        {
            return new QualifiedIdentifierStatementExpression
                {
                    Identifier = QualifiedIdentifier(),
                    ExpressionPrime = QualifiedIdentifierStatementExpressionPrime()
                };
        }

        private QualifiedIdentifierStatementExpressionPrime QualifiedIdentifierStatementExpressionPrime()
        {
            if (CheckTokenType(TokenType.ParenthesisOpen))
            {
                NextToken();
                var thisMethodCall = new ThisMethodCall{ArgumentList = ArgumentList()};
                if (!CheckTokenType(TokenType.ParenthesisClose))
                    throw new ParenthesisClosedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                thisMethodCall.CallAccess = CallAccess();
                return thisMethodCall;
            }
            if (IsIncrementDecrementOperator())
            {
                return new ThisStatementExpressionPostIncrementDecrement { IncrementDecrement = IncrementDecrement() };
            }
            if (CheckTokenType(TokenType.SquareBracketOpen))
            {
                NextToken();
                return QualifiedIdentifierStatementExpressionPrimePrime();
            }
            else if (IsAssignmentOperator())
            {
                return new StatementExpressionAssignment
                {
                    Operator = AssignmentOperator(),
                    ExpressionValue = Expression()
                };
            }

            else throw new ParserException($"'[', assignment operator, increment/decrement operator, '(', or identifier tokens expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private QualifiedIdentifierStatementExpressionPrime QualifiedIdentifierStatementExpressionPrimePrime()
        {
            if (CheckTokenType(TokenType.Comma) || CheckTokenType(TokenType.SquareBracketClose))
            {
                var thisArray = new ThisArrayRankSpecifierList
                {
                    RankSpecifier = new List<int>{ OptionalCommaList() }
                };
                if (!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                thisArray.RankSpecifier.AddRange(OptionalRankSpecifierList());
                //TODO revisar esto
                //VariableDeclaratorList();
                return thisArray;
            }
            if (IsUnaryExpression())
            {
                var arrayAccessModification = new ArrayAccessModification{ArrayAccessExpressionList = ExpressionList() };
                if (!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                arrayAccessModification.ArrayAccess = ArrayAccess();
                arrayAccessModification.ArrayOperation = QualifiedIdentifierStatementExpressionPrimePrimePrime();
                return arrayAccessModification;
            }

            throw new ParserException($"Unary expression or ',' expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private ArrayAccessIncrementDecrementAssignment QualifiedIdentifierStatementExpressionPrimePrimePrime()
        {
            //Array access increment-decrement
            if (IsIncrementDecrementOperator())
            {
                return new ArrayAccessIncrementDecrement { IncrementDecrementOperator = IncrementDecrement() };
            }

            //Array access assignment
            if (IsAssignmentOperator())
            {
                return new ArrayAccessAssignment
                {
                    Operator = AssignmentOperator(),
                    ExpressionValue = Expression()
                };
            }

            throw new ParserException($"increment/decrement or assignment operator expected at row {GetTokenRow()} column {GetTokenColumn()}.");
        }

        private ArrayAccess ArrayAccess()
        {
            if (CheckTokenType(TokenType.SquareBracketOpen))
            {
                var arrayAccess = new ArrayAccess();
                NextToken();
                arrayAccess.ExpressionList = ExpressionList();
                if (!CheckTokenType(TokenType.SquareBracketClose))
                    throw new SquareBracketCloseExpectedException(GetTokenRow(), GetTokenColumn());

                NextToken();
                arrayAccess.Access = ArrayAccess();
            }

            return new ArrayAccess();
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

        private List<Statement> OptionalStatementList()
        {
            if (IsStatementExpression() || IsEmptyBlock() || IsSelectionStatement() || IsIterationStatement() ||
                IsJumpStatement())
            {
                return StatementList();
            }
            
            return new List<Statement>();
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
