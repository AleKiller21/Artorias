using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.TablesMetadata
{
    public class UsingDirectiveTable
    {
        //TODO Semantic: Harcodear los using directives de System
        public static Dictionary<string, List<string> > Directives = new Dictionary<string, List<string>>();
    }
}
