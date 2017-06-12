using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes
{
    public class IdentifierAttribute : LineNumbering
    {
        public List<string> Identifiers;

        public IdentifierAttribute(int row, int col)
        {
            Identifiers = new List<string>();
            Row = row;
            Col = col;
        }
    }
}
