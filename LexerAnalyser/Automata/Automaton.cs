using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using LexerAnalyser.Interfaces;
using LexerAnalyser.Models;

namespace LexerAnalyser.Automata
{
    public partial class Automaton
    {
        private readonly IInputStream _inputStream;
        private Dictionary<string, TokenType> _operatorsDictionary;
        private Dictionary<char, TokenType> _punctuatorsDictionary;
        private Symbol _currentSymbol;

        public Automaton(IInputStream inputStream)
        {
            _inputStream = inputStream;
            _currentSymbol = _inputStream.GetNextSymbol();
            InitEscapeSecuenceDictionary();
        }

        public Token GetToken()
        {
            while (_currentSymbol.Character != '\0')
            {
                if (Char.IsLetter(_currentSymbol.Character)) return GetOpenToken();
                if (Char.IsDigit(_currentSymbol.Character)) return GetNumLiteralToken();
                if (_currentSymbol.Character == '\'') return GetCharToken();
                if (_currentSymbol.Character == '\"') return GetStringToken();
                if (_currentSymbol.Character == '@') return GetVerbatimStringToken();

                _currentSymbol = _inputStream.GetNextSymbol();
            }

            return new Token("\0", TokenType.Eof, _currentSymbol.RowCount, _currentSymbol.ColCount);
        }

        private Token GetOpenToken()
        {
            Token token = GetIdToken();

            //TODO soportar null literal
            if(token.Lexeme.Equals("true")) return new Token(token.Lexeme, TokenType.LiteralTrue, token.Row, token.Column);
            if(token.Lexeme.Equals("false")) return new Token(token.Lexeme, TokenType.LiteralFalse, token.Row, token.Column);

            return token;
        }

        private Token GetIdToken()
        {
            //TODO permitir que el id pueda empezar con _
            //TODO validar si el id formado no es una palabra reservada.
            //BUG tirar una excepcion cuando se ingrese caracteres no permitidos como \
            var lexeme = new StringBuilder();
            var rowCount = _currentSymbol.RowCount;
            var colCount = _currentSymbol.ColCount;
            
            while (Char.IsLetterOrDigit(_currentSymbol.Character))
            {
                lexeme.Append(_currentSymbol.Character);
                _currentSymbol = _inputStream.GetNextSymbol();
            }

            return new Token(lexeme.ToString(), TokenType.Id, rowCount, colCount);
        }

        private void ConsumeSymbol(StringBuilder lexeme)
        {
            lexeme.Append(_currentSymbol.Character);
            _currentSymbol = _inputStream.GetNextSymbol();
        }

        private void InitializeOperatorsDictionary()
        {
            _operatorsDictionary = new Dictionary<string, TokenType>();

            _operatorsDictionary["+"] = TokenType.Addition;
            _operatorsDictionary["-"] = TokenType.Subtract;
            _operatorsDictionary["*"] = TokenType.Multiply;
            _operatorsDictionary["/"] = TokenType.Division;
            _operatorsDictionary["&"] = TokenType.LogicalAnd;
            _operatorsDictionary["|"] = TokenType.LogicalOr;
            _operatorsDictionary["^"] = TokenType.LogicalXor;
            _operatorsDictionary["!"] = TokenType.LogicalNegation;
            _operatorsDictionary["~"] = TokenType.BitwiseNegation;
            _operatorsDictionary["<<"] = TokenType.ShiftLeft;
            _operatorsDictionary[">>"] = TokenType.ShiftRight;
            _operatorsDictionary["++"] = TokenType.Increment;
            _operatorsDictionary["--"] = TokenType.Decrement;
            _operatorsDictionary["??"] = TokenType.NullCoalescing;
            _operatorsDictionary["as"] = TokenType.AsType;
            _operatorsDictionary["is"] = TokenType.IsType;
            _operatorsDictionary["="] = TokenType.Assignment;
            _operatorsDictionary["%"] = TokenType.Modulo;
            _operatorsDictionary["<="] = TokenType.LessThanOrEqual;
            _operatorsDictionary[">="] = TokenType.GreaterThanOrEqual;
            _operatorsDictionary["&&"] = TokenType.ConditionalAnd;
            _operatorsDictionary["||"] = TokenType.ConditionalOr;
            _operatorsDictionary["=="] = TokenType.Equal;
            _operatorsDictionary["!="] = TokenType.NotEqual;
            _operatorsDictionary["<"] = TokenType.LessThan;
            _operatorsDictionary[">"] = TokenType.GreaterThan;
            _operatorsDictionary["?"] = TokenType.Conditional;
            _operatorsDictionary["+="] = TokenType.AddEqual;
            _operatorsDictionary["-="] = TokenType.SubtractEqual;
            _operatorsDictionary["*="] = TokenType.MultiplyEqual;
            _operatorsDictionary["/="] = TokenType.DivideEqual;
            _operatorsDictionary["%="] = TokenType.ModuloEqual;
            _operatorsDictionary["&="] = TokenType.AmpersandEqual;
            _operatorsDictionary["|="] = TokenType.OrEqual;
            _operatorsDictionary["^="] = TokenType.XorEqual;
            _operatorsDictionary["<<="] = TokenType.ShiftLeftEqual;
            _operatorsDictionary[">>="] = TokenType.ShiftRightEqual;
        }

        private void InitializePunctuatorsDictionary()
        {
            _punctuatorsDictionary = new Dictionary<char, TokenType>();

            _punctuatorsDictionary['{'] = TokenType.CurlyBraceOpen;
            _punctuatorsDictionary['}'] = TokenType.CurlyBraceClose;
            _punctuatorsDictionary['['] = TokenType.SquareBracketOpen;
            _punctuatorsDictionary[']'] = TokenType.SquareBracketClose;
            _punctuatorsDictionary['('] = TokenType.ParenthesisOpen;
            _punctuatorsDictionary[')'] = TokenType.ParenthesisClose;
            _punctuatorsDictionary['.'] = TokenType.MemberAccess;
            _punctuatorsDictionary[','] = TokenType.Comma;
            _punctuatorsDictionary[':'] = TokenType.Colon;
            _punctuatorsDictionary[';'] = TokenType.EndStatement;
        }
    }
}
