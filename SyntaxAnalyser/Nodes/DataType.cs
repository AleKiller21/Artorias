using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.Nodes
{
    public class DataType
    {
        public BuiltInDataType Type;
        public QualifiedIdentifier Name;
        public List<DataType> GenericTypes;
        public List<int> RankSpecifiers;

        public DataType()
        {
            GenericTypes = new List<DataType>();
            RankSpecifiers = new List<int>();
        }
    }
}
