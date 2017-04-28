using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Models;

namespace LexerAnalyser.Interfaces
{
    internal interface IInputStream
    {
        Token GetNextToken();
    }
}
