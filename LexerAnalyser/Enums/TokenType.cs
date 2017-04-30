using System;
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
        LiteralRegularString
    }
}
