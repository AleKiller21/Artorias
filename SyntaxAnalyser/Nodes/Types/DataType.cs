using System.Collections.Generic;

namespace SyntaxAnalyser.Nodes.Types
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
