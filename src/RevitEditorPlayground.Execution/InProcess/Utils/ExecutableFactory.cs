using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Execution.InProcess.Utils;

public static class ExecutableFactory
{
    extension(Executable)
    {
        public static Result<Executable> FromCompiledCode(
            AbsolutePath workingDirectory,
            ExecutableName executableName,
            CompiledCode code)
        {
            try
            {
                var executableFullPath = workingDirectory.Combine(executableName);
                var executableBytes = code.Bytes;
                
                File.WriteAllBytes(path:  executableFullPath, bytes: executableBytes.ToArray());
                
                return new Executable(Path: executableFullPath, Bytes: executableBytes);
            }
            catch (Exception e)
            {
                return Error.FailedToCreateExecutable(e, workingDirectory, executableName);
            }
        }
    }
}