using System.Collections.Generic;
using LexerAnalyser;
using LexerAnalyser.Enums;
using LexerAnalyser.Models;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Nodes;
using SyntaxAnalyser.Nodes.Expressions.Literal;
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

            return new List<UsingNamespaceDeclaration>();
        }

        private List<UsingNamespaceDeclaration> UsingDirective()
        {
            if(!CheckTokenType(TokenType.RwUsing))
                throw new MissingUsingKeywordException(GetTokenRow(), GetTokenColumn());

            var row = GetTokenRow();
            var col = GetTokenColumn();
            NextToken();
            var usingNamespace = new UsingNamespaceDeclaration(QualifiedIdentifier())
            {
                Row = row,
                Col = col
            };

            if(!CheckTokenType(TokenType.EndStatement))
                throw new EndOfStatementException(GetTokenRow(), GetTokenColumn());

            NextToken();
            var usingNamespaceList = OptionalUsingDirective();
            usingNamespaceList.Insert(0,  usingNamespace);

            return usingNamespaceList;
        }

        private IdentifierAttribute IdentifierAttribute()
        {
            var row = GetTokenRow();
            var col = GetTokenColumn();

            if (CheckTokenType(TokenType.MemberAccess))
            {
                NextToken();
                if (!CheckTokenType(TokenType.Id))
                    throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

                var identifier = _token.Lexeme;
                NextToken();
                var identifiers = IdentifierAttribute();
                identifiers.Identifiers.Insert(0, identifier);
                identifiers.Row = row;
                identifiers.Col = col;

                return identifiers;
            }

            return new IdentifierAttribute(row, col);
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
            return new List<TypeDeclaration>();
        }

        private TypeDeclaration TypeDeclaration()
        {
            var row = GetTokenRow();
            var col = GetTokenColumn();
            var modifier = EncapsulationModifier();
            var @type = GroupDeclaration();

            @type.Modifier = modifier;
            @type.Row = row;
            @type.Col = col;
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
            return new List<QualifiedIdentifier>();
        }

        private QualifiedIdentifier QualifiedIdentifier()
        {
            if(!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            var row = GetTokenRow();
            var col = GetTokenColumn();
            var identifier = _token.Lexeme;
            NextToken();
            var identifiers = IdentifierAttribute();
            identifiers.Identifiers.Insert(0, identifier);

            var qualifiedIdentifier = new QualifiedIdentifier
            {
                Identifiers = identifiers,
                Row = row,
                Col = col
            };
            return qualifiedIdentifier;
        }

        private bool IsBuiltInType()
        {
            return CheckTokenType(TokenType.RwInt) ||
                   CheckTokenType(TokenType.RwChar) ||
                   CheckTokenType(TokenType.RwString) ||
                   CheckTokenType(TokenType.RwBool) ||
                   CheckTokenType(TokenType.RwFloat) ||
                   CheckTokenType(TokenType.RwObject);
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
            if (CheckTokenType(TokenType.RwObject))
            {
                NextToken();
                return BuiltInDataType.Object;
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
                var row = GetTokenRow();
                var col = GetTokenColumn();
                NextToken();
                return new DataType
                {
                    BuiltInDataType = BuiltInDataType.Void,
                    Row = row,
                    Col = col
                };
            }

            throw new MissingDataTypeForIdentifierToken(GetTokenRow(), GetTokenColumn());
        }

        private DataType TypeOrVar()
        {
            if(IsType()) return Type();
            if (CheckTokenType(TokenType.RwOrIdVar))
            {
                var row = GetTokenRow();
                var col = GetTokenColumn();
                NextToken();
                return new DataType
                {
                    BuiltInDataType = BuiltInDataType.Var,
                    Row = row,
                    Col = col
                };
            }

            throw new MissingDataTypeForIdentifierToken(GetTokenRow(), GetTokenColumn());
        }

        private DataType Type()
        {
            var type = new DataType{Row = GetTokenRow(), Col = GetTokenColumn()};

            NonArrayType(type);
            type.GenericTypes = OptionalGeneric();
            type.RankSpecifiers = OptionalRankSpecifierList();

            return type;
        }

        private List<DataType> OptionalGeneric()
        {
            if(CheckTokenType(TokenType.OpLessThan)) return Generic();
            return new List<DataType>();
        }

        private void NonArrayType(DataType type)
        {
            if (CheckTokenType(TokenType.Id))
            {
                type.CustomTypeName = QualifiedIdentifier();
                type.BuiltInDataType = BuiltInDataType.None;
            }
            else if (IsBuiltInType())
            {
                type.BuiltInDataType = BuiltInType();
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

            return new List<FixedParameter>();
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
            return new List<FixedParameter>();
        }

        private FixedParameter FixedParameter()
        {
            var parameter = new FixedParameter
            {
                Row = GetTokenRow(),
                Col = GetTokenColumn(),
                Type = Type()
            };

            if (!CheckTokenType(TokenType.Id))
                throw new IdTokenExpectecException(GetTokenRow(), GetTokenColumn());

            parameter.Identifier = _token.Lexeme;
            NextToken();

            return parameter;
        }

        private LiteralExpression Literal()
        {
            if (IsLiteral())
            {
                LiteralExpression literal;

                if(IsLiteralInt()) literal = new IntLiteral(int.Parse(_token.Lexeme), GetTokenRow(), GetTokenColumn());
                else if(IsLiteralString()) literal = new StringLiteral(_token.Lexeme, GetTokenRow(), GetTokenColumn());
                else if(CheckTokenType(TokenType.LiteralCharSimple)) literal = new CharLiteral(_token.Lexeme.ToCharArray()[0], GetTokenRow(), GetTokenColumn());
                else if(CheckTokenType(TokenType.LiteralFloat)) literal = new FloatLiteral(float.Parse(_token.Lexeme.Substring(0, _token.Lexeme.Length-1)), GetTokenRow(), GetTokenColumn());
                else if(CheckTokenType(TokenType.LiteralTrue)) literal = new BooleanLiteral(true, GetTokenRow(), GetTokenColumn());
                else literal = new BooleanLiteral(false, GetTokenRow(), GetTokenColumn());

                NextToken();
                return literal;
            }

            throw new LiteralExpectedException(GetTokenRow(), GetTokenColumn());
        }
    }
}
