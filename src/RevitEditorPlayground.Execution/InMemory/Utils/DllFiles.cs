using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Execution.InMemory.Utils;

public static class DllFiles
{
    extension(DllFile)
    {
        public static Result<IReadOnlyList<DllFile>> FromDependencies(AbsolutePath scriptDirectory,
            IEnumerable<CompilationDependency> dependencies)
        {
            try
            {
                return dependencies
                    .Select(dependency =>
                    {
                        var nameWithExtension = Path.GetFileName(dependency.Path);
                        var targetPath = Path.Combine(scriptDirectory, nameWithExtension);
                        
                        File.Copy(sourceFileName: dependency.Path, targetPath, overwrite: true);

                        return new DllFile(targetPath);
                    })
                    .ToList();
            }
            catch (Exception e)
            {
                return Error.UnexpectedDependencyDllCreation(e);
            }

        }
        
        public static Result<DllFile> FromCompiledCode(CompiledCode compiledCode, AbsolutePath path)
        {
            try
            {
                var bytes = compiledCode.Bytes.ToArray();
                File.WriteAllBytes(path, bytes);
                
                return new DllFile(path);
                
            }
            catch (Exception e)
            {
                return Error.UnexpectedMainDllCreation(e);
            }
        }
    }
}