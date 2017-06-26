using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes.Types;

namespace SyntaxAnalyser.Nodes.Expressions.Binary.Equality
{
    public abstract class EqualityOperator : BinaryOperator
    {
        protected EqualityOperator()
        {
            Rules["bool,bool"] = new BoolType();
            Rules["char,char"] = new BoolType();
            Rules["char,int"] = new BoolType();
            Rules["char,float"] = new BoolType();

            Rules["int,char"] = new BoolType();
            Rules["int,int"] = new BoolType();
            Rules["int,float"] = new BoolType();

            Rules["float,char"] = new BoolType();
            Rules["float,int"] = new BoolType();
            Rules["float,float"] = new BoolType();

            Rules["string,string"] = new BoolType();
        }
    }
}
