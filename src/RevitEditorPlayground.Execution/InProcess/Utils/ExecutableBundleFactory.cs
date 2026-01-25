using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Execution.InMemory;
using RevitEditorPlayground.Execution.InMemory.Utils;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Execution.InProcess.Utils;

public static class ExecutableBundleFactory
{
    extension(ExecutableBundle executableBundle)
    {
        public static Result<ExecutableBundle> From(
            CompiledCode code,
            IReadOnlyList<CompilationDependency> dependencies,
            AbsolutePath workingDirectory,
            ExecutableName executableName
            )
        {
            return Executable.FromCompiledCode(workingDirectory, executableName, code)
                .Then(executable =>
                {
                    var localDependencies = dependencies
                        .Where(dependency => dependency.Kind == DependencyKind.CopyLocal);
            
                    return DllFile.FromDependencies(
                        scriptDirectory: workingDirectory,
                        dependencies: localDependencies
                    ).WithContext(executable);
                })
                .Map(input =>
                {
                    var executable = input.Context;
                    var dlls = input.Value;
                    return new ExecutableBundle(
                        Executable: executable,
                        Dependencies: dlls,
                        WorkingDirectory: workingDirectory
                    );
                });
        }
        
        public Result<ExecutableBundle> Delete()
        {
            try
            {
                File.Delete(executableBundle.Executable.Path);
                foreach (var dependency in executableBundle.Dependencies)
                {
                    File.Delete(dependency);
                }
                
                return executableBundle;
            }
            catch (Exception e)
            {
                return Error.UnexpectedExecutableDeletion(e);
            }
        }
    }
}