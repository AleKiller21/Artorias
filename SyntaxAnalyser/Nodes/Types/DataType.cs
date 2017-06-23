using System.Collections.Generic;

namespace SyntaxAnalyser.Nodes.Types
{
    public class DataType : LineNumbering
    {
        public BuiltInDataType BuiltInDataType;
        public QualifiedIdentifier CustomTypeName;
        public List<DataType> GenericTypes;
        public List<int> RankSpecifiers;
        //TODO Semantic: Arrays for later

        public DataType()
        {
            GenericTypes = new List<DataType>();
            RankSpecifiers = new List<int>();
        }
    }
}
