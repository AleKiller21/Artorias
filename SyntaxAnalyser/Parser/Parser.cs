using System;
using System.Collections.Generic;
using LexerAnalyser;
using LexerAnalyser.Enums;
using LexerAnalyser.Models;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes;
using SyntaxAnalyser.Nodes.Namespaces;
using SyntaxAnalyser.Nodes.Types;

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

        private List<TypeDeclaration> TypeDeclarationList()
        {
            if (HasEncapsulationModifier() || (CheckTokenType(TokenType.RwAbstract) || CheckTokenType(TokenType.RwClass))
                || CheckTokenType(TokenType.RwInterface) || CheckTokenType(TokenType.RwEnum))
            {
                var typeDeclaration = TypeDeclaration();
                var typeDeclarationList = TypeDeclarationList();

                typeDeclarationList.Insert(0, typeDeclaration);
                return typeDeclarationList;
            }
            else
            {
                return new List<TypeDeclaration>();
            }
        }

        private TypeDeclaration TypeDeclaration()
        {
            var modifier = EncapsulationModifier();
            var @type = GroupDeclaration();

            @type.Modifier = modifier;
            return @type;
        }

        private TypeDeclaration GroupDeclaration()
        {
            if (CheckTokenType(TokenType.RwAbstract) || CheckTokenType(TokenType.RwClass)) return ClassDeclaration();
            if (CheckTokenType(TokenType.RwInterface)) return InterfaceDeclaration();
            if (CheckTokenType(TokenType.RwEnum)) return EnumDeclaration();

            throw new ParserException($"Class, Interface or Enum identifier expected at row {GetTokenRow()} column {GetTokenColumn()}");
        }

        private AccessModifier EncapsulationModifier()
        {
            if (CheckTokenType(TokenType.RwPublic))
            {
                NextToken();
                return AccessModifier.Public;
            }
            if (CheckTokenType(TokenType.RwPrivate))
            {
                NextToken();
                return AccessModifier.Private;
            }
            if (CheckTokenType(TokenType.RwProtected))
            {
                NextToken();
                return AccessModifier.Protected;
            }

            return AccessModifier.None;
        }

        private void OptionalBodyEnd()
        {
            if(CheckTokenType(TokenType.EndStatement)) NextToken();
            else
            {
                //Epsilon
            }
        }

        private List<QualifiedIdentifier> IdentifiersList()
        {
            var qualifiedIdentifier = QualifiedIdentifier();
            var identifierList = IdentifierListPrime();

            identifierList.Insert(0, qualifiedIdentifier);
            return identifierList;
        }

        private List<QualifiedIdentifier> IdentifierListPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                var qualifiedIdentifier = QualifiedIdentifier();
                var identifierList = IdentifierListPrime();
                identifierList.Insert(0, qualifiedIdentifier);

                return identifierList;
            }
            else
            {
                return new List<QualifiedIdentifier>();
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

        private BuiltInDataType BuiltInType()
        {
            if (CheckTokenType(TokenType.RwInt))
            {
                NextToken();
                return BuiltInDataType.Int;
            }
            if (CheckTokenType(TokenType.RwChar))
            {
                NextToken();
                return BuiltInDataType.Char;
            }
            if (CheckTokenType(TokenType.RwString))
            {
                NextToken();
                return BuiltInDataType.String;
            }
            if (CheckTokenType(TokenType.RwBool))
            {
                NextToken();
                return BuiltInDataType.Bool;
            }
            if (CheckTokenType(TokenType.RwFloat))
            {
                NextToken();
                return BuiltInDataType.Float;
            }

            throw new BuiltInDataTypeException(GetTokenRow(), GetTokenColumn());
        }

        private DataType TypeOrVoid()
        {
            if (IsType()) return Type();
            if (CheckTokenType(TokenType.RwVoid))
            {
                NextToken();
                return new DataType {Type = BuiltInDataType.Void};
            }

            throw new MissingDataTypeForIdentifierToken(GetTokenRow(), GetTokenColumn());
        }

        private DataType TypeOrVar()
        {
            if(IsType()) return Type();
            if (CheckTokenType(TokenType.RwOrIdVar))
            {
                NextToken();
                return new DataType {Type = BuiltInDataType.Var};
            }

            throw new MissingDataTypeForIdentifierToken(GetTokenRow(), GetTokenColumn());
        }

        private DataType Type()
        {
            var type = new DataType();

            NonArrayType(type);
            type.GenericTypes = OptionalGeneric();
            type.RankSpecifiers = OptionalRankSpecifierList();

            return type;
        }

        private List<DataType> OptionalGeneric()
        {
            if(CheckTokenType(TokenType.OpLessThan)) return Generic();
            else
            {
                return new List<DataType>();
            }
        }

        private void NonArrayType(DataType type)
        {
            if (CheckTokenType(TokenType.Id))
            {
                type.Name = QualifiedIdentifier();
                type.Type = BuiltInDataType.None;
            }
            else if (IsBuiltInType())
            {
                type.Type = BuiltInType();
            }
            else throw new MissingDataTypeForIdentifierToken(GetTokenRow(), GetTokenColumn());
        }

        private List<FixedParameter> FixedParameters()
        {
            if (IsType())
            {
                var parameter = FixedParameter();
                var parameters = FixedParametersPrime();

                parameters.Insert(0, parameter);
                return parameters;
            }

            else
            {
                return new List<FixedParameter>();
            }
        }

        private List<FixedParameter> FixedParametersPrime()
        {
            if (CheckTokenType(TokenType.Comma))
            {
                NextToken();
                var parameter = FixedParameter();
                var parameters = FixedParametersPrime();

                parameters.Insert(0, parameter);
                return parameters;
            }
            else
            {
                return new List<FixedParameter>();
            }
        }

        private FixedParameter FixedParameter()
        {
            var parameter = new FixedParameter();
            parameter.Type = Type();

            if (!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            parameter.Identifier = _token.Lexeme;
            NextToken();

            return parameter;
        }

        private void Literal()
        {
            if (IsLiteral()) NextToken();
            else throw new LiteralExpectedException(GetTokenRow(), GetTokenColumn());
        }
    }
}
