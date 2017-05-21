using System;
using LexerAnalyser;
using LexerAnalyser.Enums;
using LexerAnalyser.Models;
using SyntaxAnalyser.Exceptions;

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

        private void NextToken()
        {
            _token = _lexer.GetToken();
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

        private bool HasEncapsulationModifier()
        {
            return CheckTokenType(TokenType.RwPublic) || CheckTokenType(TokenType.RwPrivate) ||
                   CheckTokenType(TokenType.RwProtected);
        }

        private bool IsTypeOrVoid()
        {
            return CheckTokenType(TokenType.RwVoid) || IsType();
        }

        private bool IsType()
        {
           return CheckTokenType(TokenType.RwInt) ||
                CheckTokenType(TokenType.RwChar) ||
                CheckTokenType(TokenType.RwString) ||
                CheckTokenType(TokenType.RwBool) ||
                CheckTokenType(TokenType.RwFloat) ||
                CheckTokenType(TokenType.Id);
        }

        public void Parse()
        {
            Code();
            if(!CheckTokenType(TokenType.Eof)) throw new ParserException($"End of file expected at row {GetTokenRow()} column {GetTokenColumn()}");
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
                throw new MissingUsingKeywordException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            NextToken();
            IdentifierAttribute();

            if(!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();
            OptionalUsingDirective();
        }

        private void IdentifierAttribute()
        {
            if (CheckTokenType(TokenType.MemberAccess))
            {
                NextToken();
                if (!CheckTokenType(TokenType.Id))
                    throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

                NextToken();
                IdentifierAttribute();
            }

            else
            {
                //Epsilon
            }
        }

        private void TypeDeclarationList()
        {
            if (HasEncapsulationModifier() || (CheckTokenType(TokenType.RwAbstract) || CheckTokenType(TokenType.RwClass))
                || CheckTokenType(TokenType.RwInterface) || CheckTokenType(TokenType.RwEnum))
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
            if(HasEncapsulationModifier()) EncapsulationModifier();
            GroupDeclaration();
        }

        private void GroupDeclaration()
        {
            //TODO ClassDeclaration
            if (CheckTokenType(TokenType.RwInterface)) InterfaceDeclaration();
        }

        private void ClassDeclaration()
        {
            //TODO
        }

        private void EnumDeclaration()
        {
            //TODO
        }

        private void EncapsulationModifier()
        {
            if (CheckTokenType(TokenType.RwPublic)) NextToken();
            else if (CheckTokenType(TokenType.RwPrivate)) NextToken();
            else if (CheckTokenType(TokenType.RwProtected)) NextToken();
            else
            {
                //Epsilon
            }
        }

        private void OptionalBodyEnd()
        {
            if(CheckTokenType(TokenType.EndStatement)) NextToken();
            else
            {
                //Epsilon
            }
        }

        private void IdentifiersList()
        {
            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            NextToken();
            if(CheckTokenType(TokenType.Comma)) IdentifierListPrime();
        }

        private void IdentifierListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                if(!CheckTokenType(TokenType.Id)) throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());
                NextToken();
                IdentifierListPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private void TypeOrVoid()
        {
            if (!IsTypeOrVoid()) throw new MissingDataTypeForIdentifierToken(GetTokenRow(), GetTokenColumn());

            NextToken();
        }

        private void Type()
        {
            if(!IsType()) throw new MissingDataTypeForIdentifierToken(GetTokenRow(), GetTokenColumn());

            NextToken();
        }

        private void FixedParameters()
        {
            if (IsType())
            {
                FixedParameter();
                if(CheckTokenType(TokenType.Comma)) FixedParametersPrime();
            }

            else
            {
                //Epsilon
            }
        }

        private void FixedParametersPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                FixedParameter();
                FixedParametersPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private void FixedParameter()
        {
            Type();

            if (!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            NextToken();
        }
    }
}
