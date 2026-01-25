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
            var errors = new List<Error>();
            
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
                    var workingDirectory = options.WorkingDirectory;
                    
                    var executableName = options.ExecutableName
                        .OrElse(defaultValue:  ExecutableName.FromKnownString(
                            name: $"main_{Guid.NewGuid().ToString()}.exe"
                            ));
                    
                    events.Add(DomainEvent.CompilationEnded(compiledCode));

                    events.Add(DomainEvent.StartingExecutableBundleCreation());

                    return ExecutableBundle.From(
                        code: compiledCode,
                        workingDirectory: workingDirectory,
                        executableName: executableName,
                        dependencies: options.Dependencies
                    ).WithContext(compiledCode);

                })
                .Then(input =>
                {
                    var compiledCode = input.Context;
                    var executableBundle = input.Value;
                    events.Add(DomainEvent.CreatedExecutableBundle(executableBundle));

                    return ExecutedProcess.RunFromBundle(executableBundle)
                        .WithContext((compiledCode, executableBundle));

                })
                .Map(input =>
                {
                    var (compiledCode, executableBundle) = input.Context;
                    var executedProcess = input.Value;
                    
                    events.Add(DomainEvent.BundleExecutionEnded(executedProcess));

                    var deletedExecutableBundleResult = executableBundle.Delete();

                    deletedExecutableBundleResult
                        .Match(
                            valid: deleted =>
                            {
                                events.Add(DomainEvent.ExecutableBundleDeleted(deleted));
                            },
                            invalid: error =>
                            {
                                errors.Add(error);
                            }
                        );
                    
                    return (compiledCode, executedProcess);

                })
                .Map(input =>
                {
                    var (compiledCode, executedProcess) = input;

                    return new InProcessExecutionOutput(
                        CompiledCode: compiledCode,
                        ExecutedProcess: executedProcess,
                        Errors: errors,
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