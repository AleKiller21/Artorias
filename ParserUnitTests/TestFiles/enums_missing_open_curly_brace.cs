// Using directives
using System;
using System.Linq;

interface Test
{
    
}

enum EnumTest

    
}

// Nested namespaces
namespace X
{
    namespace A
    {
        namespace B
        {
            namespace C
            {
                using System.Console;
            }
        }
    }

    namespace D
    {
    }
}

namespace Y
{
    enum Test
    {
        A, B, C, D
    }

    private enum Test2
    {
        A = 1, B, C = 4,
    }
}

namespace Z
{
    public enum Test
    {
        
    }

    interface ITest : IInterface, ISomething, IDontKnow
    {
        string doSomething(string message);
        void sayHi();
        bool truthy();
        float pi();
        int sum(int a, int b);
        SomeClass hey();
    }

    interface ITest2
    {
        void what();
    }

    interface ITest3
    {

    }

    namespace A
    {
        
    }
}

enum What
{
    
}
