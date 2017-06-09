﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LexerAnalyser.Enums
{
    public enum TokenType
    {
        Id,
        Eof,
        LiteralSimpleNum,
        LiteralBinary,
        LiteralHexadecimal,
        LiteralFloat,
        LiteralCharSimple,
        EscapeSecuenceSingleQuote,
        EscapeSecuenceDoubleQuote,
        EscapeSecuenceBackslash,
        EscapeSecuenceZero,
        EscapeSecuenceAlert,
        EscapeSecuenceBackspace,
        EscapeSecuenceFormFeed,
        EscapeSecuenceNewLine,
        EscapeSecuenceCarriageReturn,
        EscapeSecuenceHorizontalTab,
        EscapeSecuenceVerticalQuote,
        LiteralTrue,
        LiteralFalse,
        LiteralRegularString,
        LiteralVerbatimString,
        LiteralNull,
        CurlyBraceOpen,
        CurlyBraceClose,
        SquareBracketOpen,
        SquareBracketClose,
        ParenthesisOpen,
        ParenthesisClose,
        MemberAccess,
        Comma,
        Colon,
        EndStatement,
        OpAddition,
        OpSubtract,
        OpMultiply,
        OpDivision,
        OpModulo,
        OpLogicalAnd,
        OpLogicalOr,
        OpLogicalXor,
        OpLogicalNegation,
        OpBitwiseNegation,
        OpAssignment,
        OpLessThan,
        OpGreaterThan,
        OpConditional,
        OpNullCoalescing,
        OpIncrement,
        OpDecrement,
        OpConditionalAnd,
        OpConditionalOr,
        OpEqual,
        OpNotEqual,
        OpLessThanOrEqual,
        OpGreaterThanOrEqual,
        OpAddEqual,
        OpSubtractEqual,
        OpMultiplyEqual,
        OpDivideEqual,
        OpModuloEqual,
        OpAmpersandEqual,
        OpOrEqual,
        OpXorEqual,
        OpShiftLeft,
        OpShiftRight,
        OpShiftLeftEqual,
        OpShiftRightEqual,
        OpIsType,
        OpAsType,
        RwAbstract,
        RwClass,
        RwDelegate,
        RwEvent,
        RwFixed,
        RwIf,
        RwInternal,
        RwNew,
        RwOverride,
        RwReadOnly,
        RwStruct,
        RwTry,
        RwUnsafe,
        RwVolatile,
        RwCase,
        RwConst,
        RwDo,
        RwExplicit,
        RwFloat,
        RwImplicit,
        RwParams,
        RwRef,
        RwSizeOf,
        RwSwitch,
        RwTypeOf,
        RwWhile,
        RwBase,
        RwCatch,
        RwContinue,
        RwExtern,
        RwFor,
        RwIn,
        RwLock,
        RwObject,
        RwPrivate,
        RwReturn,
        RwStackalloc,
        RwThis,
        RwUsing,
        RwBool,
        RwChar,
        RwElse,
        RwFalse,
        RwForEach,
        RwInt,
        RwOperator,
        RwProtected,
        RwStatic,
        RwThrow,
        RwVirtual,
        RwBreak,
        RwChecked,
        RwDefault,
        RwEnum,
        RwFinally,
        RwGoto,
        RwInterface,
        RwNameSpace,
        RwOut,
        RwPublic,
        RwSealed,
        RwString,
        RwUnchecked,
        RwVoid,
        RwOrIdAdd,
        RwOrIdAsync,
        RwOrIdAwait,
        RwOrIdDynamic,
        RwOrIdGet,
        RwOrIdGlobal,
        RwOrIdPartial,
        RwOrIdRemove,
        RwOrIdSet,
        //RwOrIdValue,
        RwOrIdVar,
        RwOrIdWhere,
        RwOrId
    }
}
