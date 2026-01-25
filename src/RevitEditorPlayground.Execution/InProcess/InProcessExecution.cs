using Microsoft.CodeAnalysis;
using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Compilation.Utils;
using RevitEditorPlayground.Execution.InProcess.Utils;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared.Events;

namespace RevitEditorPlayground.Execution.InProcess;

public static class InProcessExecution
{
    public static Result<InProcessExecutionOutput> Run(
        RawCodebase codebase,
        InProcessExecutionOptions options)
    {
        try
        {
            var events = new List<DomainEvent>();
            
            var packages = options.Dependencies
                .Select(dependency => MetadataReference.CreateFromFile(dependency.Path))
                .ToList();
            
            return CompileOptions.FromFrameworkVersion(
                frameworkVersion: options.Framework,
                languageVersion: options.LanguageVersion,
                packages: packages,
                outputKind: OutputKind.ConsoleApplication)
                .Then(compileOptions =>
                {
                    var assemblyName = options.AssemblyName;
                    events.Add(DomainEvent.CompilationStarted(compileOptions, assemblyName));

                    return CompiledCode.FromCodebase(
                        codebase: codebase,
                        assemblyName: assemblyName,
                        compileOptions: compileOptions
                    );
                })
                .Then(compiledCode =>
                {
                    var executablePath = options.ExecutablePath;
                    events.Add(DomainEvent.CompiledCode(compiledCode));

                    events.Add(DomainEvent.StartingExecutableCreation());
                    
                    return PhysicalExecutable.FromOptionalPath(optionalPath: executablePath, compiledCode: compiledCode)
                        .WithContext(compiledCode);
                })
                .Then(input =>
                {
                    var compiledCode = input.Context;
                    var physicalExecutable = input.Value;
                    events.Add(DomainEvent.CreatedExecutable(physicalExecutable));

                    return System.Diagnostics.Process.StartExecutable(physicalExecutable)
                        .WithContext((CompiledCode: compiledCode, PhysicalExecutable: physicalExecutable));
                })
                .Map(input =>
                {
                    var compiledCode = input.Context.CompiledCode;
                    var physicalExecutable = input.Context.PhysicalExecutable;
                    var process = input.Value;

                    events.Add(DomainEvent.ProcessStarted(process));
                    var stdout = process.StandardOutput.ReadToEnd();
                    var stderr = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    events.Add(DomainEvent.ProcessExited(process));

                    var outputs = (StandardOutput: stdout, StandardError: stderr);

                    return outputs.WithContext((CompiledCode: compiledCode, PhysicalExecutable: physicalExecutable));
                })
                .Map(input =>
                {
                    var (compiledCode, physicalExecutable) = input.Context;
                    var (standardOutput, standardError) = input.Value;

                    physicalExecutable.Delete()
                        .Do(deletedExecutable =>
                        {
                            events.Add(DomainEvent.ExecutableDeleted(deletedExecutable));
                        });

                    return new InProcessExecutionOutput(
                        CompiledCode: compiledCode,
                        StandardOutput: standardOutput,
                        StandardError: standardError,
                        Events: events
                    );
                });
            
        }
        catch (Exception e)
        {
            return Error.UnexpectedProcessFailure(e);
        }
    }
}