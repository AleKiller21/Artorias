﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes.Statements.JumpStatements
{
    public abstract class JumpStatement : Statement
    {
        public override void ValidateSemantic()
        {
            throw new NotImplementedException();
        }

        public override string GenerateJS()
        {
            throw new NotImplementedException();
        }
    }
}
