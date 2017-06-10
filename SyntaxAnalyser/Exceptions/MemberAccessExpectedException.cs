using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Exceptions
{
    public class MemberAccessExpectedException : Exception
    {
        public MemberAccessExpectedException(int row, int col) : base($"'.' token expected at row {row} column {col}.")
        {
            
        }
    }
}
