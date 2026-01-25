using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared.Errors;

namespace RevitEditorPlayground.Execution.InProcess.Utils;

public static class ExecutableNameFactory
{
    extension(ExecutableName)
    {
        public static Result<ExecutableName> FromString(string name)
        {
            try
            {
                var extension = Path.GetExtension(name);

                if (extension != ".exe")
                {
                    return Error.Failure(description: "Executable name must have .exe extension");
                }

                return new ExecutableName(name);
            }
            catch (Exception e)
            {
                return Error.Failure(
                    description: "Failed to create executable name",
                    metadata: Error.ExceptionMetadata(e)
                );
                ;
            }
        }
        
        internal static ExecutableName FromKnownString(string name)
        {
            return new ExecutableName(name);
        }
    }
}