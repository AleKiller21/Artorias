using System;
using System.Collections.Generic;
using LexerAnalyser;
using LexerAnalyser.Enums;
using LexerAnalyser.Models;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes;

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

        private bool IsLiteralInt()
        {
            return CheckTokenType(TokenType.LiteralSimpleNum) || 
                    CheckTokenType(TokenType.LiteralBinary) || 
                    CheckTokenType(TokenType.LiteralHexadecimal);
        }

        private bool IsLiteralString()
        {
            return CheckTokenType(TokenType.LiteralRegularString) || CheckTokenType(TokenType.LiteralVerbatimString);
        }

        private bool IsLiteral()
        {
            return IsLiteralInt() ||
                   IsLiteralString() ||
                   CheckTokenType(TokenType.LiteralCharSimple) ||
                   CheckTokenType(TokenType.LiteralFloat) ||
                   CheckTokenType(TokenType.LiteralTrue) ||
                   CheckTokenType(TokenType.LiteralFalse);
        }

        private bool HasOptionalModifier()
        {
            return CheckTokenType(TokenType.RwStatic) ||
                   CheckTokenType(TokenType.RwVirtual) ||
                   CheckTokenType(TokenType.RwOverride) ||
                   CheckTokenType(TokenType.RwAbstract);
        }

        private bool IsGroupDeclaration()
        {
            return CheckTokenType(TokenType.RwAbstract) || CheckTokenType(TokenType.RwClass) ||
                   CheckTokenType(TokenType.RwInterface) || CheckTokenType(TokenType.RwEnum);
        }

        public Code Parse()
        {
            var code = Code();
            if(!CheckTokenType(TokenType.Eof)) throw new ParserException($"End of file expected at row {GetTokenRow()} column {GetTokenColumn()}");

            return code;
        }

        private Code Code()
        {
            var code = new Code();

            if (CheckTokenType(TokenType.RwUsing))
            {
                code.GlobalNamespace.UsingNamespaces = UsingDirective();
                OptionalNameSpaceMemberDeclaration(code.GlobalNamespace);

                return code;
            }

            if (CheckTokenType(TokenType.RwNameSpace) || HasEncapsulationModifier() || IsGroupDeclaration())
            {
                NamespaceMemberDeclaration(code.GlobalNamespace);

                return code;
            }

            return code;
        }

        private List<UsingNamespaceDeclaration> OptionalUsingDirective()
        {
            if (CheckTokenType(TokenType.RwUsing))
            {
                return UsingDirective();
            }

            else
            {
                return new List<UsingNamespaceDeclaration>();
            }
        }

        private List<UsingNamespaceDeclaration> UsingDirective()
        {
            if(!CheckTokenType(TokenType.RwUsing))
                throw new MissingUsingKeywordException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var usingNamespace = new UsingNamespaceDeclaration(QualifiedIdentifier());

            if(!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var usingNamespaceList = OptionalUsingDirective();
            usingNamespaceList.Insert(0,  usingNamespace);

            return usingNamespaceList;
        }

        private List<string> IdentifierAttribute()
        {
            if (CheckTokenType(TokenType.MemberAccess))
            {
                NextToken();
                if (!CheckTokenType(TokenType.Id))
                    throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

                var identifier = _token.Lexeme;
                NextToken();
                var identifiers = IdentifierAttribute();
                identifiers.Insert(0, identifier);

                return identifiers;
            }

            else
            {
                return new List<string>();
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
            EncapsulationModifier();
            GroupDeclaration();
        }

        private void GroupDeclaration()
        {
            if(CheckTokenType(TokenType.RwAbstract) || CheckTokenType(TokenType.RwClass)) ClassDeclaration();
            else if (CheckTokenType(TokenType.RwInterface)) InterfaceDeclaration();
            else if (CheckTokenType(TokenType.RwEnum)) EnumDeclaration();
            else
                throw new ParserException($"Class, Interface or Enum identifier expected at row {GetTokenRow()} column {GetTokenColumn()}");
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
            QualifiedIdentifier();
            IdentifierListPrime();
        }

        private void IdentifierListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                QualifiedIdentifier();
                IdentifierListPrime();
            }
            else
            {
                //Epsilon
            }
        }

        private QualifiedIdentifier QualifiedIdentifier()
        {
            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            var identifier = _token.Lexeme;
            NextToken();
            var identifiers = IdentifierAttribute();
            identifiers.Insert(0, identifier);

            var qualifiedIdentifier = new QualifiedIdentifier {Identifiers = identifiers};
            return qualifiedIdentifier;
        }

        private bool IsBuiltInType()
        {
            return CheckTokenType(TokenType.RwInt) ||
                   CheckTokenType(TokenType.RwChar) ||
                   CheckTokenType(TokenType.RwString) ||
                   CheckTokenType(TokenType.RwBool) ||
                   CheckTokenType(TokenType.RwFloat);
        }

        private void BuiltInType()
        {
            if(IsBuiltInType()) NextToken();
            else throw new BuiltInDataTypeException(GetTokenRow(), GetTokenColumn());
        }

        private void TypeOrVoid()
        {
            if (IsType()) Type();
            else if(CheckTokenType(TokenType.RwVoid)) NextToken();

            else throw new MissingDataTypeForIdentifierToken(GetTokenRow(), GetTokenColumn());
        }

        private void TypeOrVar()
        {
            if(IsType()) Type();
            else if(CheckTokenType(TokenType.RwOrIdVar)) NextToken();
            else throw new MissingDataTypeForIdentifierToken(GetTokenRow(), GetTokenColumn());
        }

        private void Type()
        {
            NonArrayType();
            OptionalGeneric();
            OptionalRankSpecifierList();
        }

        private void OptionalGeneric()
        {
            if(CheckTokenType(TokenType.OpLessThan)) Generic();
            else
            {
                //Epsilon
            }
        }

        private void NonArrayType()
        {
            if(CheckTokenType(TokenType.Id)) QualifiedIdentifier();
            else if(IsBuiltInType()) BuiltInType();
            else throw new MissingDataTypeForIdentifierToken(GetTokenRow(), GetTokenColumn());
        }

        private void FixedParameters()
        {
            if (IsType())
            {
                FixedParameter();
                FixedParametersPrime();
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

        private void Literal()
        {
            if (IsLiteral()) NextToken();
            else throw new LiteralExpectedException(GetTokenRow(), GetTokenColumn());
        }
    }
}
