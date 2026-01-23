using Microsoft.CodeAnalysis;
using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Compilation.Utils;
using RevitEditorPlayground.Execution.InProcess;
using RevitEditorPlayground.Functional;
using Shouldly;

namespace RevitEditorPlayground.Execution.Tests.InProcess;

public static class InProcessExecutionTestsUtils
{
    extension(InProcessExecution)
    {
        public static InProcessExecutionOutput RunRawCode(string code)
        {
            var inProcessResult = CompilationContext.FromRawCode(
                    code: [code],
                    assemblyName: "MyApp",
                    frameworkVersion: FrameworkVersion.Net48,
                    outputKind: OutputKind.ConsoleApplication
                )
                .Then(static context =>
                {
                    var (codebase, assemblyName, compileOptions) = context;
                
                    var inProcessContext = new InProcessExecutionContext(
                        RawCodebase: codebase,
                        AssemblyName: assemblyName,
                        CompileOptions: compileOptions,
                        ExecutablePath: Option.None);
                
                    return InProcessExecution.Run(inProcessContext);
                });

            inProcessResult.IsValid.ShouldBeTrue();

            inProcessResult.Value.ShouldNotBeNull();

            return inProcessResult.Value!;
        }
    }
    
    extension(InProcessExecutionOutput output)
    {
        public object Normalize()
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