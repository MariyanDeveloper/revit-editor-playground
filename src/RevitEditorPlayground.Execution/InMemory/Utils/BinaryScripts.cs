using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Execution.InMemory.Utils;

public static class BinaryScripts
{
    extension(BinaryScript)
    {

        public static Result<BinaryScript> From(
            AbsolutePath scriptDirectory,
            CompiledCode compiledCode,
            IReadOnlyList<CompilationDependency> dependencies
        )
        {
            var assemblyName = compiledCode.Compilation.AssemblyName.NotNull();
            var mainDllFileName = $"{assemblyName}.dll";
            var mainDllFullPath = scriptDirectory.Combine(mainDllFileName);
            
            return DllFile.FromCompiledCode(compiledCode, mainDllFullPath)
                .Then(mainDll =>
                {
                    var localDependencies = dependencies
                        .Where(dependency => dependency.Kind == DependencyKind.CopyLocal);
                    
                    return DllFile.FromDependencies(scriptDirectory: scriptDirectory, dependencies: localDependencies)
                        .WithContext(mainDll);
                })
                .Map(input =>
                {
                    var dependencyDlls = input.Value;
                    var mainDll = input.Context;
                    
                    return new BinaryScript(Directory: scriptDirectory, Main: mainDll, Dependencies: dependencyDlls);
                });
            
        }
    }
}