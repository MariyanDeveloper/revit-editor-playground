using System.Reflection;
using Microsoft.CodeAnalysis;
using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Compilation.Utils;
using RevitEditorPlayground.Execution.InMemory.Utils;
using RevitEditorPlayground.Functional;

namespace RevitEditorPlayground.Execution.InMemory;


public static class InMemoryExecution
{
    public static Result<InMemoryExecutionOutput> Run(
        RawCodebase rawCodebase,
        InMemoryExecutionOptions executionOptions)
    {
        try
        {
            var packages = executionOptions.Dependencies
                .Select(dependency => MetadataReference.CreateFromFile(dependency.Path))
                .ToList();

            return CompileOptions.FromFrameworkVersion(
                    frameworkVersion: executionOptions.Framework,
                    languageVersion: executionOptions.LanguageVersion,
                    packages: packages,
                    outputKind: OutputKind.DynamicallyLinkedLibrary
                )
                .Then(compileOptions =>
                {
                    var assemblyName = executionOptions.AssemblyName;

                    return CompiledCode.FromCodebase(
                        codebase: rawCodebase,
                        assemblyName: assemblyName,
                        compileOptions: compileOptions
                    );
                })
                .Then(compiledCode =>
                {
                    var scriptDirectory = executionOptions.ScriptDirectory;

                    return ScriptBundle.From(scriptDirectory: scriptDirectory, compiledCode: compiledCode,
                            dependencies: executionOptions.Dependencies)
                        .WithContext(compiledCode);
                })
                .Then(input =>
                {
                    var compiledCode = input.Context;
                    var binaryScript = input.Value;

                    return Assembly.LoadPlugin(pluginPath: binaryScript.Main)
                        .WithContext(compiledCode);
                })
                .Then(input =>
                {
                    var compiledCode = input.Context;
                    var assembly = input.Value;

                    return Script.FromAssembly(assembly)
                        .WithContext((compiledCode, assembly));
                })
                .Then(input =>
                {
                    var script = input.Value;
                    var optionalArgs = executionOptions.Args;

                    return script.Execute(optionalArgs)
                        .WithContext(input.Context);
                })
                .Map(input =>
                {
                    var (compiledCode, assembly) = input.Context;
                    var executedScript = input.Value;
                    
                    return new InMemoryExecutionOutput(
                        CompiledCode: compiledCode,
                        Assembly: assembly,
                        ExecutedScript: executedScript,
                        Events: []
                    );
                });
        }
        catch (Exception e)
        {
            return Error.UnexpectedInMemoryExecutionFailure(e);
        }
    }
}