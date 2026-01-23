using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Execution.InProcess.Utils;

public static class PhysicalExecutables
{
    extension(PhysicalExecutable physicalExecutable)
    {
        public static Result<PhysicalExecutable> TemporaryFromCompiledCode(CompiledCode compiledCode)
        {
            try
            {
                var fileName = $"myapp_{Guid.NewGuid():N}.exe";

                var absolutePath = AbsolutePath.FromCurrentDirectory(
                    fileName: fileName
                );

                File.WriteAllBytes(absolutePath, compiledCode.Bytes.ToArray());

                return new PhysicalExecutable(compiledCode.Bytes, absolutePath);
            }
            catch (Exception e)
            {
                return Error.UnexpectedExecutableCreation(e);
            }
            
        }

        public Result<PhysicalExecutable> Delete()
        {
            try
            {
                File.Delete(physicalExecutable.Path);
                return physicalExecutable;
            }
            catch (Exception e)
            {
                return Error.UnexpectedExecutableDeletion(e);
            }
        }
    }
}