using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using LexerAnalyser.Models;

namespace LexerAnalyser.Automata
{
    public partial class Automaton
    {
        private Token GetOperatorToken()
        {
            TokenType type;
            try
            {
                type = _operatorsDictionary[_currentSymbol.Character.ToString()];
            }
            catch (KeyNotFoundException e)
            {
                return null;
            }

            switch (type)
            {
                case TokenType.Addition:
                    return GetCompoundOperatorToken(TokenType.Addition);
                case TokenType.Subtract:
                    return GetCompoundOperatorToken(TokenType.Subtract);
;                case TokenType.Multiply:
                    return GetCompoundOperatorToken(TokenType.Multiply);
                case TokenType.Division:
                    return GetCompoundOperatorToken(TokenType.Division);
                case TokenType.LogicalAnd:
                    return GetCompoundOperatorToken(TokenType.LogicalAnd);
                case TokenType.LogicalOr:
                    return GetCompoundOperatorToken(TokenType.LogicalOr);
                case TokenType.LogicalXor:
                    return GetCompoundOperatorToken(TokenType.LogicalXor);
                case TokenType.LogicalNegation:
                    return GetCompoundOperatorToken(TokenType.LogicalNegation);
                case TokenType.Assignment:
                    return GetCompoundOperatorToken(TokenType.Assignment);
                case TokenType.Modulo:
                    return GetCompoundOperatorToken(TokenType.Modulo);
                case TokenType.LessThan:
                    return GetCompoundOperatorToken(TokenType.LessThan);
                case TokenType.GreaterThan:
                    return GetCompoundOperatorToken(TokenType.GreaterThan);
                case TokenType.Conditional:
                    return GetCompoundOperatorToken(TokenType.Conditional);
                case TokenType.BitwiseNegation:
                    return GetCompoundOperatorToken(TokenType.BitwiseNegation);
                default:
                    return null;
            }
        }

        private Token GetCompoundOperatorToken(TokenType type)
        {
            var lexeme = new StringBuilder("" + _currentSymbol.Character);
            var row = _currentSymbol.RowCount;
            var col = _currentSymbol.ColCount;

            while (true)
            {
                _currentSymbol = _inputStream.GetNextSymbol();
                if(Char.IsWhiteSpace(_currentSymbol.Character)) continue;
                try
                {
                    type = _operatorsDictionary[lexeme.ToString() + _currentSymbol.Character.ToString()];
                    lexeme.Append(_currentSymbol.Character);
                }
                catch (KeyNotFoundException e)
                {
                    return new Token(lexeme.ToString(), type, row, col);
                }
            }
        }

        private void InitializeOperatorsDictionary()
        {
            _operatorsDictionary = new Dictionary<string, TokenType>
            {
                ["+"] = TokenType.Addition,
                ["-"] = TokenType.Subtract,
                ["*"] = TokenType.Multiply,
                ["/"] = TokenType.Division,
                ["&"] = TokenType.LogicalAnd,
                ["|"] = TokenType.LogicalOr,
                ["^"] = TokenType.LogicalXor,
                ["!"] = TokenType.LogicalNegation,
                ["~"] = TokenType.BitwiseNegation,
                ["="] = TokenType.Assignment,
                ["%"] = TokenType.Modulo,
                ["<"] = TokenType.LessThan,
                [">"] = TokenType.GreaterThan,
                ["?"] = TokenType.Conditional,
                ["<<"] = TokenType.ShiftLeft,
                [">>"] = TokenType.ShiftRight,
                ["++"] = TokenType.Increment,
                ["--"] = TokenType.Decrement,
                ["??"] = TokenType.NullCoalescing,
                ["as"] = TokenType.AsType,
                ["is"] = TokenType.IsType,
                ["<="] = TokenType.LessThanOrEqual,
                [">="] = TokenType.GreaterThanOrEqual,
                ["&&"] = TokenType.ConditionalAnd,
                ["||"] = TokenType.ConditionalOr,
                ["=="] = TokenType.Equal,
                ["!="] = TokenType.NotEqual,
                ["+="] = TokenType.AddEqual,
                ["-="] = TokenType.SubtractEqual,
                ["*="] = TokenType.MultiplyEqual,
                ["/="] = TokenType.DivideEqual,
                ["%="] = TokenType.ModuloEqual,
                ["&="] = TokenType.AmpersandEqual,
                ["|="] = TokenType.OrEqual,
                ["^="] = TokenType.XorEqual,
                ["<<="] = TokenType.ShiftLeftEqual,
                [">>="] = TokenType.ShiftRightEqual
            };

        }
    }
}
