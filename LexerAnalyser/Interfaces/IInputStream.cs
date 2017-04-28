using System;
using System.Collections.Generic;
using System.Text;
using LexerAnalyser.Models;

namespace LexerAnalyser.Interfaces
{
    public interface IInputStream
    {
        Symbol GetNextSymbol();
    }
}
