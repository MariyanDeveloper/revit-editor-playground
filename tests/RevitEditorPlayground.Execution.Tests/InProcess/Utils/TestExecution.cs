using Microsoft.CodeAnalysis.CSharp;
using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Compilation.Utils;
using RevitEditorPlayground.Execution.InProcess;
using RevitEditorPlayground.Execution.InProcess.Utils;
using RevitEditorPlayground.Execution.Shared;
using RevitEditorPlayground.Execution.Tests.Utils;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Execution.Tests.InProcess.Utils;

public static class TestExecution
{
    extension(InProcessExecution)
    {
        public static Result<InProcessExecutionOutput> RunTest(
            string code,
            string? assemblyName = null,
            IReadOnlyList<CompilationDependency>? dependencies = null
        )
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var testDirectory = $"test_{Guid.NewGuid().ToString()}";
            var testPath = Path.Combine(currentDirectory, testDirectory);

            Directory.CreateDirectory(testPath);

            var workingDirectory = AbsolutePath.FromExistingDirectory(testPath)
                .ShouldHaveValue();
            
            Directory.CreateDirectory(testPath);


            var options = new InProcessExecutionOptions(
                AssemblyName: AssemblyName.FromString(assemblyName ?? "MyApp").ShouldHaveValue(),
                Framework: FrameworkVersion.Net48,
                LanguageVersion: LanguageVersion.CSharp14,
                Dependencies: dependencies ?? [],
                WorkingDirectory: workingDirectory,
                ExecutableName: ExecutableName.FromString(
                    name: "test_app.exe"
                ).ShouldHaveValue()
                );
            
            var codebase = RawCodebase.FromRawCode(code).ShouldHaveValue();
            
            return InProcessExecution.Run(codebase, options);

        }
    }
}