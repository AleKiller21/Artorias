using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Enums;
using LexerAnalyser.Exceptions;
using LexerAnalyser.Interfaces;
using LexerAnalyser.Models;

namespace LexerAnalyser.Automata
{
    public partial class Automaton
    {
        private readonly IInputStream _inputStream;
        private Dictionary<string, TokenType> _operatorsDictionary;
        private Dictionary<char, TokenType> _punctuatorsDictionary;
        private Dictionary<string, TokenType> _reservedWordsDictionary;
        private Symbol _currentSymbol;

        public Automaton(IInputStream inputStream)
        {
            _inputStream = inputStream;
            _currentSymbol = _inputStream.GetNextSymbol();
            InitEscapeSecuenceDictionary();
            InitializePunctuatorsDictionary();
            InitializeOperatorsDictionary();
            InitializeReservedWordsDictionary();
        }

        public Token GetToken()
        {
            while (_currentSymbol.Character != '\0')
            {
                if (Char.IsWhiteSpace(_currentSymbol.Character))
                {
                    _currentSymbol = _inputStream.GetNextSymbol();
                    continue;
                }
                if(SkipComments()) continue;

                if (Char.IsLetter(_currentSymbol.Character) || _currentSymbol.Character == '_') return GetOpenToken();
                if (Char.IsDigit(_currentSymbol.Character)) return GetNumLiteralToken();
                if (_currentSymbol.Character == '\'') return GetCharToken();
                if (_currentSymbol.Character == '\"') return GetStringToken();
                if (_currentSymbol.Character == '@') return GetVerbatimStringToken();

                var token = GetPunctuatorToken();
                if (token != null) return token;

                token = GetOperatorToken();
                if (token != null) return token;

                throw new LexicalException(String.Format("Unrecognized token at row {0} column {1}.", _currentSymbol.RowCount, _currentSymbol.ColCount));
            }

            return new Token("\0", TokenType.Eof, _currentSymbol.RowCount, _currentSymbol.ColCount);
        }

        private bool SkipComments()
        {
            if(_currentSymbol.Character == '/' && _inputStream.PeekNextSymbol().Character == '/') return SkipLineComment();
            if (_currentSymbol.Character == '/' &&
                     _inputStream.PeekNextSymbol().Character == '*') return SkipBlockComment(_currentSymbol.RowCount, _currentSymbol.ColCount);

            return false;
        }

        private bool SkipBlockComment(int row, int col)
        {
            _currentSymbol = _inputStream.GetNextSymbol();

            do
            {
                _currentSymbol = _inputStream.GetNextSymbol();
                if(_currentSymbol.Character == '\0')
                    throw new LexicalException(String.Format("End of file found. The block-line comment at row {0} column {1} was never closed.", row, col));
            } while (_currentSymbol.Character != '*' || _inputStream.PeekNextSymbol().Character != '/');

            _currentSymbol = _inputStream.GetNextSymbol();
            _currentSymbol = _inputStream.GetNextSymbol();
            return true;
        }

        private bool SkipLineComment()
        {
            do
            {
                _currentSymbol = _inputStream.GetNextSymbol();
            } while (_currentSymbol.Character != '\n' && _currentSymbol.Character != '\0');

            _currentSymbol = _inputStream.GetNextSymbol();
            return true;
        }

        private Token GetOpenToken()
        {
            var idToken = GetIdToken();
            var openToken = GetSpecialLiteralToken(idToken);
            if (openToken != null) return openToken;

            openToken = GetSpecialOperatorToken(idToken);
            if (openToken != null) return openToken;

            openToken = GetReservedWordToken(idToken);
            return openToken ?? idToken;
        }

        private Token GetReservedWordToken(Token idToken)
        {
            try
            {
                TokenType type = _reservedWordsDictionary[idToken.Lexeme];
                return new Token(idToken.Lexeme, type, idToken.Row, idToken.Column);
            }
            catch (KeyNotFoundException e)
            {
                return null;
            }
        }

        private Token GetSpecialLiteralToken(Token idToken)
        {
            if (idToken.Lexeme.Equals("true")) return new Token(idToken.Lexeme, TokenType.LiteralTrue, idToken.Row, idToken.Column);
            if (idToken.Lexeme.Equals("false")) return new Token(idToken.Lexeme, TokenType.LiteralFalse, idToken.Row, idToken.Column);
            return idToken.Lexeme.Equals("null") ? new Token(idToken.Lexeme, TokenType.LiteralNull, idToken.Row, idToken.Column) : null;
        }

        private Token GetIdToken()
        {
            //TODO Aceptar Unicode characters (opcional)
            var lexeme = new StringBuilder();
            var rowCount = _currentSymbol.RowCount;
            var colCount = _currentSymbol.ColCount;
            
            while (Char.IsLetterOrDigit(_currentSymbol.Character) || _currentSymbol.Character == '_')
            {
                lexeme.Append(_currentSymbol.Character);
                _currentSymbol = _inputStream.GetNextSymbol();
            }

            return new Token(lexeme.ToString(), TokenType.Id, rowCount, colCount);
        }

        private Token GetPunctuatorToken()
        {
            try
            {
                var type = _punctuatorsDictionary[_currentSymbol.Character];
                var row = _currentSymbol.RowCount;
                var col = _currentSymbol.ColCount;
                var lexeme = _currentSymbol.Character;

                _currentSymbol = _inputStream.GetNextSymbol();
                return new Token(lexeme.ToString(), type, row, col);
            }
            catch (KeyNotFoundException e)
            {
                return null;
            }
        }

        private void ConsumeSymbol(StringBuilder lexeme)
        {
            lexeme.Append(_currentSymbol.Character);
            _currentSymbol = _inputStream.GetNextSymbol();
        }

        private void InitializePunctuatorsDictionary()
        {
            _punctuatorsDictionary = new Dictionary<char, TokenType>
            {
                ['{'] = TokenType.CurlyBraceOpen,
                ['}'] = TokenType.CurlyBraceClose,
                ['['] = TokenType.SquareBracketOpen,
                [']'] = TokenType.SquareBracketClose,
                ['('] = TokenType.ParenthesisOpen,
                [')'] = TokenType.ParenthesisClose,
                ['.'] = TokenType.MemberAccess,
                [','] = TokenType.Comma,
                [':'] = TokenType.Colon,
                [';'] = TokenType.EndStatement
            };

        }

        private void InitializeReservedWordsDictionary()
        {
            _reservedWordsDictionary = new Dictionary<string, TokenType>
            {
                ["abstract"] = TokenType.RwAbstract,
                ["class"] = TokenType.RwClass,
                ["delegate"] = TokenType.RwDelegate,
                ["event"] = TokenType.RwEvent,
                ["fixed"] = TokenType.RwFixed,
                ["if"] = TokenType.RwIf,
                ["internal"] = TokenType.RwInternal,
                ["new"] = TokenType.RwNew,
                ["override"] = TokenType.RwOverride,
                ["readonly"] = TokenType.RwReadOnly,
                ["struct"] = TokenType.RwStruct,
                ["try"] = TokenType.RwTry,
                ["unsafe"] = TokenType.RwUnsafe,
                ["volatile"] = TokenType.RwVolatile,
                ["case"] = TokenType.RwCase,
                ["const"] = TokenType.RwConst,
                ["do"] = TokenType.RwDo,
                ["explicit"] = TokenType.RwExplicit,
                ["float"] = TokenType.RwFloat,
                ["implicit"] = TokenType.RwImplicit,
                ["params"] = TokenType.RwParams,
                ["ref"] = TokenType.RwRef,
                ["sizeof"] = TokenType.RwSizeOf,
                ["switch"] = TokenType.RwSwitch,
                ["typeof"] = TokenType.RwTypeOf,
                ["while"] = TokenType.RwWhile,
                ["base"] = TokenType.RwBase,
                ["catch"] = TokenType.RwCatch,
                ["continue"] = TokenType.RwContinue,
                ["extern"] = TokenType.RwExtern,
                ["for"] = TokenType.RwFor,
                ["in"] = TokenType.RwIn,
                ["lock"] = TokenType.RwLock,
                //["object"] = TokenType.RwObject,
                ["private"] = TokenType.RwPrivate,
                ["return"] = TokenType.RwReturn,
                ["stackalloc"] = TokenType.RwStackalloc,
                ["this"] = TokenType.RwThis,
                ["using"] = TokenType.RwUsing,
                ["bool"] = TokenType.RwBool,
                ["char"] = TokenType.RwChar,
                ["else"] = TokenType.RwElse,
                ["false"] = TokenType.RwFalse,
                ["foreach"] = TokenType.RwForEach,
                ["int"] = TokenType.RwInt,
                ["operator"] = TokenType.RwOperator,
                ["protected"] = TokenType.RwProtected,
                ["static"] = TokenType.RwStatic,
                ["throw"] = TokenType.RwThrow,
                ["virtual"] = TokenType.RwVirtual,
                ["break"] = TokenType.RwBreak,
                ["checked"] = TokenType.RwChecked,
                ["default"] = TokenType.RwDefault,
                ["enum"] = TokenType.RwEnum,
                ["finally"] = TokenType.RwFinally,
                ["goto"] = TokenType.RwGoto,
                ["interface"] = TokenType.RwInterface,
                ["namespace"] = TokenType.RwNameSpace,
                ["out"] = TokenType.RwOut,
                ["public"] = TokenType.RwPublic,
                ["sealed"] = TokenType.RwSealed,
                ["string"] = TokenType.RwString,
                ["unchecked"] = TokenType.RwUnchecked,
                ["void"] = TokenType.RwVoid,
                ["add"] = TokenType.RwOrIdAdd,
                ["async"] = TokenType.RwOrIdAsync,
                ["await"] = TokenType.RwOrIdAwait,
                ["dynamic"] = TokenType.RwOrIdDynamic,
                ["get"] = TokenType.RwOrIdGet,
                ["global"] = TokenType.RwOrIdGlobal,
                ["partial"] = TokenType.RwOrIdPartial,
                ["remove"] = TokenType.RwOrIdRemove,
                ["set"] = TokenType.RwOrIdSet,
                ["value"] = TokenType.RwOrIdValue,
                ["var"] = TokenType.RwOrIdVar,
                ["where"] = TokenType.RwOrIdWhere,
                ["yield"] = TokenType.RwOrId
            };
        }
    }
}
