using System;
using LexerAnalyser;
using LexerAnalyser.Enums;
using LexerAnalyser.Models;

namespace SyntaxAnalyser.Parser
{
    public partial class Parser
    {
        private Lexer _lexer;
        private Token _token;

        public Parser(Lexer lexer)
        {
            _lexer = lexer;
            _token = _lexer.GetToken();
        }

        private Token GetNextToken()
        {
            return _lexer.GetToken();
        }

        private int GetTokenRow()
        {
            return _token.Row;
        }

        private int GetTokenColumn()
        {
            return _token.Column;
        }

        private bool CheckTokenType(TokenType type)
        {
            return _token.Type == type;
        }

        public void Parse()
        {
            Code();
            if(!CheckTokenType(TokenType.Eof)) throw new ParserException($"End of file expected at row {GetTokenRow()} column");
        }

        private void Code()
        {
            CompilationUnit();
        }

        private void CompilationUnit()
        {
            if (CheckTokenType(TokenType.RwUsing))
            {
                OptionalUsingDirective();
                OptionalNameSpaceMemberDeclaration();
            }

            else if (CheckTokenType(TokenType.RwNameSpace))
            {
                OptionalNameSpaceMemberDeclaration();
            }

            else
            {
                throw new ParserException($"using keyword or namespace keyword expected at row {GetTokenRow()} column {GetTokenColumn()}");
            }
        }

        private void OptionalUsingDirective()
        {
            if (CheckTokenType(TokenType.RwUsing))
            {
                UsingDirective();
            }

            else
            {
                //Epsilon
            }
        }

        private void UsingDirective()
        {
            if(!CheckTokenType(TokenType.RwUsing))
                throw new ParserException($"using keyword expected at row {GetTokenRow()} column {GetTokenColumn()}");

            _token = GetNextToken();
            if(!CheckTokenType(TokenType.Id))
                throw new ParserException($"identifier token expected at row {GetTokenRow()} column {GetTokenColumn()}");

            _token = GetNextToken();
            IdentifierAttribute();

            if(!CheckTokenType(TokenType.EndStatement))
                throw new ParserException($"End of statement expected at row {GetTokenRow()} column {GetTokenColumn()}");

            _token = GetNextToken();
            OptionalUsingDirective();
        }

        private void IdentifierAttribute()
        {
            if (CheckTokenType(TokenType.MemberAccess))
            {
                _token = GetNextToken();
                if (!CheckTokenType(TokenType.Id))
                    throw new ParserException($"Id token expected at row {GetTokenRow()} column {GetTokenColumn()}");

                _token = GetNextToken();
                IdentifierAttribute();
            }

            else
            {
                //Epsilon
            }
        }

        private void TypeDeclarationList()
        {
            if (CheckTokenType(TokenType.RwPublic) || CheckTokenType(TokenType.RwPrivate) ||
                CheckTokenType(TokenType.RwProtected))
            {
                TypeDeclaration();
                TypeDeclarationList();
            }
            else
            {
                //Epsilon
            }
        }

        private void TypeDeclaration()
        {
            EncapsulationModifier();
            GroupDeclaration();//Pendiente
        }

        private void GroupDeclaration()
        {
            throw new NotImplementedException();
        }

        private void EncapsulationModifier()
        {
            if (CheckTokenType(TokenType.RwPublic)) _token = GetNextToken();
            else if (CheckTokenType(TokenType.RwPrivate)) _token = GetNextToken();
            else if (CheckTokenType(TokenType.RwProtected)) _token = GetNextToken();
            else
            {
                //Epsilon
            }
        }
    }
}
