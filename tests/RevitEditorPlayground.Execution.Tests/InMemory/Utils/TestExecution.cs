using Microsoft.CodeAnalysis.CSharp;
using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Compilation.Utils;
using RevitEditorPlayground.Execution.InMemory;
using RevitEditorPlayground.Execution.InMemory.Utils;
using RevitEditorPlayground.Execution.Shared;
using RevitEditorPlayground.Execution.Tests.Utils;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Execution.Tests.InMemory.Utils;

public static class TestExecution
{
    extension(InMemoryExecution)
    {
        public static Result<InMemoryExecutionOutput> RunTest(
            string codebase,
            object?[]? args = null,
            string? assemblyName = null
        )
        {
            var testId = Guid.NewGuid().ToString();
            
            var testPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                testId
            );
            
            Directory.CreateDirectory(testPath);

 
            var rawCodebase = RawCodebase.FromRawCode(codebase)
                .ShouldHaveValue();
        
            var executionOptions = InMemoryExecutionOptions.From(
                framework: FrameworkVersion.Net48,
                languageVersion: LanguageVersion.CSharp14,
                assemblyName: AssemblyName.FromString(assemblyName ?? "TestScript").ShouldHaveValue(),
                scriptDirectory: AbsolutePath.FromExistingDirectory(testPath).ShouldHaveValue(),
                args: args,
                dependencies:
                [
                    CompilationDependency.ProvidedByHost(AbsolutePath.FromExistingFile(InMemoryLibs.Contracts)
                        .ShouldHaveValue())
                ]
            );
        
            return InMemoryExecution.Run(
                rawCodebase: rawCodebase,
                executionOptions: executionOptions
            );
      
        }
    }
}