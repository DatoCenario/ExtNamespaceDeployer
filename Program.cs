using System;
using System.IO;

namespace ExtJsNamespaceDeployer
{
    class Program
    {
        static void Main(string[] args)
        {
            var r = new ExtDefinitionsReader("C:/Users/Dato/source/repos/ExtJsNamespaceDeployer/ExtJsNamespaceDeployer/q.js");
            var d = r.ReadNextDefinition();
            d = r.ReadNextDefinition();
            d = r.ReadNextDefinition();
            d = r.ReadNextDefinition();
            int a = 0;
        }

    }
}
