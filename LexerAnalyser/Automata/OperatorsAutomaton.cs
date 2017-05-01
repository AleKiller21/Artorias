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
                case TokenType.OpAddition:
                    return GetCompoundOperatorToken(TokenType.OpAddition);
                case TokenType.OpSubtract:
                    return GetCompoundOperatorToken(TokenType.OpSubtract);
;                case TokenType.OpMultiply:
                    return GetCompoundOperatorToken(TokenType.OpMultiply);
                case TokenType.OpDivision:
                    return GetCompoundOperatorToken(TokenType.OpDivision);
                case TokenType.OpLogicalAnd:
                    return GetCompoundOperatorToken(TokenType.OpLogicalAnd);
                case TokenType.OpLogicalOr:
                    return GetCompoundOperatorToken(TokenType.OpLogicalOr);
                case TokenType.OpLogicalXor:
                    return GetCompoundOperatorToken(TokenType.OpLogicalXor);
                case TokenType.OpLogicalNegation:
                    return GetCompoundOperatorToken(TokenType.OpLogicalNegation);
                case TokenType.OpAssignment:
                    return GetCompoundOperatorToken(TokenType.OpAssignment);
                case TokenType.OpModulo:
                    return GetCompoundOperatorToken(TokenType.OpModulo);
                case TokenType.OpLessThan:
                    return GetCompoundOperatorToken(TokenType.OpLessThan);
                case TokenType.OpGreaterThan:
                    return GetCompoundOperatorToken(TokenType.OpGreaterThan);
                case TokenType.OpConditional:
                    return GetCompoundOperatorToken(TokenType.OpConditional);
                case TokenType.OpBitwiseNegation:
                    return GetCompoundOperatorToken(TokenType.OpBitwiseNegation);
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
                    type = _operatorsDictionary[lexeme + _currentSymbol.Character.ToString()];
                    lexeme.Append(_currentSymbol.Character);
                }
                catch (KeyNotFoundException e)
                {
                    return new Token(lexeme.ToString(), type, row, col);
                }
            }
        }

        private Token GetSpecialOperatorToken(Token idToken)
        {
            if (idToken.Lexeme.Equals("as")) return new Token(idToken.Lexeme, TokenType.OpAsType, idToken.Row, idToken.Column);
            return idToken.Lexeme.Equals("is") ? new Token(idToken.Lexeme, TokenType.OpIsType, idToken.Row, idToken.Column) : null;
        }

        private void InitializeOperatorsDictionary()
        {
            _operatorsDictionary = new Dictionary<string, TokenType>
            {
                ["+"] = TokenType.OpAddition,
                ["-"] = TokenType.OpSubtract,
                ["*"] = TokenType.OpMultiply,
                ["/"] = TokenType.OpDivision,
                ["&"] = TokenType.OpLogicalAnd,
                ["|"] = TokenType.OpLogicalOr,
                ["^"] = TokenType.OpLogicalXor,
                ["!"] = TokenType.OpLogicalNegation,
                ["~"] = TokenType.OpBitwiseNegation,
                ["="] = TokenType.OpAssignment,
                ["%"] = TokenType.OpModulo,
                ["<"] = TokenType.OpLessThan,
                [">"] = TokenType.OpGreaterThan,
                ["?"] = TokenType.OpConditional,
                ["<<"] = TokenType.OpShiftLeft,
                [">>"] = TokenType.OpShiftRight,
                ["++"] = TokenType.OpIncrement,
                ["--"] = TokenType.OpDecrement,
                ["??"] = TokenType.OpNullCoalescing,
                ["as"] = TokenType.OpAsType,
                ["is"] = TokenType.OpIsType,
                ["<="] = TokenType.OpLessThanOrEqual,
                [">="] = TokenType.OpGreaterThanOrEqual,
                ["&&"] = TokenType.OpConditionalAnd,
                ["||"] = TokenType.OpConditionalOr,
                ["=="] = TokenType.OpEqual,
                ["!="] = TokenType.OpNotEqual,
                ["+="] = TokenType.OpAddEqual,
                ["-="] = TokenType.OpSubtractEqual,
                ["*="] = TokenType.OpMultiplyEqual,
                ["/="] = TokenType.OpDivideEqual,
                ["%="] = TokenType.OpModuloEqual,
                ["&="] = TokenType.OpAmpersandEqual,
                ["|="] = TokenType.OpOrEqual,
                ["^="] = TokenType.OpXorEqual,
                ["<<="] = TokenType.OpShiftLeftEqual,
                [">>="] = TokenType.OpShiftRightEqual
            };

        }
    }
}
