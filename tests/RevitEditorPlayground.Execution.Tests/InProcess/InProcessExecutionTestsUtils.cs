using Microsoft.CodeAnalysis.CSharp;
using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Compilation.Utils;
using RevitEditorPlayground.Execution.InProcess;
using RevitEditorPlayground.Execution.Tests.Utils;
using RevitEditorPlayground.Functional;
using Shouldly;

namespace RevitEditorPlayground.Execution.Tests.InProcess;

public static class InProcessExecutionTestsUtils
{
    extension(InProcessExecution)
    {
        public static InProcessExecutionOutput RunRawCode(string code)
        {
            var options = new InProcessExecutionOptions(
                AssemblyName: AssemblyName.FromString("MyApp").ShouldHaveValue(),
                Framework: FrameworkVersion.Net48,
                LanguageVersion: LanguageVersion.CSharp14,
                Dependencies: [],
                ExecutablePath: Option.None);
            
            var codebase = RawCodebase.FromRawCode(code).ShouldHaveValue();
            
            var inProcessResult = InProcessExecution.Run(codebase, options);

            inProcessResult.IsValid.ShouldBeTrue();

            inProcessResult.Value.ShouldNotBeNull();

            return inProcessResult.Value!;
        }
    }
    
    extension(InProcessExecutionOutput output)
    {
        public object SnapshotNormalized()
        {
            
            return new
            {
                output.StandardError,
                output.StandardOutput,
                Events = output.Events.Select(e => new
                {
                    e.Message,
                    e.Code,
                    e.Level
                }).ToList()
            };
        }
    }
}