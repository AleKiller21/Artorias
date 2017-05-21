// Using directives
using System;
using System.Linq;

interface Test
{
    
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
}

namespace Z
{
    interface ITest : IInterface, ISomething, IDontKnow
    {
        string doSomething(string message);
        void sayHi();
        truthy();
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