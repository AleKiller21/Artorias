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
        Addition,
        Subtract,
        Multiply,
        Division,
        Modulo,
        LogicalAnd,
        LogicalOr,
        LogicalXor,
        LogicalNegation,
        BitwiseNegation,
        Assignment,
        LessThan,
        GreaterThan,
        Conditional,
        NullCoalescing,
        Increment,
        Decrement,
        PreIncrement,
        PreDecrement,
        PostIncrement,
        PostDecrement,
        ConditionalAnd,
        ConditionalOr,
        Equal,
        NotEqual,
        LessThanOrEqual,
        GreaterThanOrEqual,
        AddEqual,
        SubtractEqual,
        MultiplyEqual,
        DivideEqual,
        ModuloEqual,
        AmpersandEqual,
        OrEqual,
        XorEqual,
        ShiftLeft,
        ShiftRight,
        ShiftLeftEqual,
        ShiftRightEqual,
        IsType,
        AsType
    }
}
