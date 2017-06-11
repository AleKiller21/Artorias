using System;
using System.Collections.Generic;
using System.Text;
using SyntaxAnalyser.Nodes;
using SyntaxAnalyser.Nodes.Types;

namespace SemanticAnalyser
{
    public class NamespaceTable
    {
        //TODO Semantic: Agregar el namespace de System.Object por default
        public static readonly Dictionary<string, List<TypeDeclaration>> Dictionary = new Dictionary<string, List<TypeDeclaration>>();
    }
}
