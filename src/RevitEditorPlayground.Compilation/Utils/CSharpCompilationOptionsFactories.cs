using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace RevitEditorPlayground.Compilation.Utils;

public static class CSharpCompilationOptionsFactories
{
    extension(CSharpCompilationOptions)
    {
        public static CSharpCompilationOptions Dll()
        {
            return new CSharpCompilationOptions(outputKind: OutputKind.DynamicallyLinkedLibrary);
        }

        public static CSharpCompilationOptions Exe(string[]? usings = null)
        {
            return new CSharpCompilationOptions(
                outputKind: OutputKind.ConsoleApplication,
                usings: usings
            );
        }
    }
}
