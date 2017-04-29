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
        LiteralHexadecimal
    }
}
