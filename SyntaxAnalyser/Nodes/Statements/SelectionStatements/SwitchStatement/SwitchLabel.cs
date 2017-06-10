using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Expressions;

namespace SyntaxAnalyser.Nodes.Statements.SwitchStatement
{
    public class SwitchLabel
    {
        public Label Label;
        public Expression Expression; //Solo cuando el label es 'case'
    }
}
