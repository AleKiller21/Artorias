using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Classes
{
    public class VariableInitializer : LineNumbering
    {
        public Expression Expression;
        public List<VariableInitializer> ArrayInitializers;

        public VariableInitializer(int row, int col)
        {
            ArrayInitializers = new List<VariableInitializer>();
            Row = row;
            Col = col;
        }
    }
}
