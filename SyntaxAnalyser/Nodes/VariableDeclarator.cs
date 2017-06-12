using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Classes;

namespace SyntaxAnalyser.Nodes
{
    public class VariableDeclarator
    {
        public string Identifier;
        public VariableInitializer VariableInitializer;
        public int row;
        public int col;
    }
}
