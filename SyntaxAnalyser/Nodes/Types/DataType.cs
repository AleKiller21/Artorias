using System.Collections.Generic;
using SyntaxAnalyser.TablesMetadata;

namespace SyntaxAnalyser.Nodes.Types
{
    public class DataType : LineNumbering
    {
        public BuiltInDataType BuiltInDataType;
        public QualifiedIdentifier CustomTypeName;
        public bool IsVar;
        public bool IsVoid;
        public List<DataType> GenericTypes;
        public List<int> RankSpecifiers;
        //TODO Semantic: Arrays for later

        public DataType()
        {
            GenericTypes = new List<DataType>();
            RankSpecifiers = new List<int>();
        }

        public Type EvaluateType()
        {
            switch (BuiltInDataType)
            {
                case BuiltInDataType.Bool:
                    return new BoolType();
                case BuiltInDataType.Char:
                    return new CharType();
                case BuiltInDataType.Float:
                    return new FloatType();
                case BuiltInDataType.Int:
                    return new IntType();
                case BuiltInDataType.Object:
                    return new ObjectType();
                case BuiltInDataType.String:
                    return new StringType();
                default:
                    return SymbolTable.GetInstance().FindType(string.Join(".", CustomTypeName.Identifiers.Identifiers));
            }
        }
    }
}
