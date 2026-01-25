using Microsoft.CodeAnalysis.CSharp;
using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Execution.Shared;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Execution.InMemory.Utils;

public static class InMemoryExecutionOptionsFactory
{
    extension(InMemoryExecutionOptions)
    {
        public static InMemoryExecutionOptions From(
            FrameworkVersion framework,
            LanguageVersion languageVersion,
            AssemblyName assemblyName,
            AbsolutePath scriptDirectory,
            IReadOnlyList<CompilationDependency>? dependencies = null,
            object?[]? args = null
            )
        {
            return new InMemoryExecutionOptions(
                AssemblyName: assemblyName,
                Framework: framework,
                LanguageVersion: languageVersion,
                Dependencies: dependencies ?? [],
                ScriptDirectory: scriptDirectory,
                Args: Option.FromOptional(args)
            );
        }
    }
}