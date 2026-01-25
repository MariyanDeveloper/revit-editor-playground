using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared.Errors;

namespace RevitEditorPlayground.Execution.InMemory;

public static  class Errors
{
    extension(Error)
    {
        public static Error NoScriptsFound()
        {
            return Error.Failure(
                code: ErrorCodes.NoScriptsFound,
                description: "No scripts found"
            );
        }
        public static Error MultipleScriptsFound(List<ScriptMethod> foundScripts)
        {
            var scriptsCount = foundScripts.Count;

            var scriptsMetadata = foundScripts
                .Select(script => new
                {
                    script.Attribute.Name,
                    Type = script.Method.DeclaringType?.FullName,
                    Method = script.Method.Name
                })
                .ToList();
            
            return Error.Failure(
                code: ErrorCodes.MultipleScriptsFound,
                description: $"Found {scriptsCount} scripts. Expected 1.",
                metadata: new Dictionary<string, object>()
                {
                    ["scripts"] = scriptsMetadata
                }
            );
        }
        public static Error UnexpectedScriptExecutionFailure(Exception exception)
        {
            return Error.Failure(
                code: ErrorCodes.UnexpectedScriptExecutionFailure,
                description: "Unexpected failure during script execution.",
                metadata: Error.ExceptionMetadata(exception)
            );
        }
        public static Error UnexpectedInMemoryExecutionFailure(Exception exception)
        {
            return Error.Failure(
                code: ErrorCodes.UnexpectedInMemoryExecutionFailure,
                description: "Unexpected failure during in-memory execution.",
                metadata: Error.ExceptionMetadata(exception)
            );
        }
        
        public static Error UnexpectedScriptResolution(Exception exception)
        {
            return Error.Failure(
                code: ErrorCodes.UnexpectedScriptResolution,
                description: "Unexpected failure during script resolution.",
                metadata: Error.ExceptionMetadata(exception)
            );
        }
        
        public static Error UnexpectedDependencyDllCreation(Exception exception)
        {
            return Error.Failure(
                code: ErrorCodes.UnexpectedDependencyDllCreation,
                description: "Unexpected failure during dependency DLL creation.",
                metadata: Error.ExceptionMetadata(exception)
            );
        }
        
        public static Error UnexpectedMainDllCreation(Exception exception)
        {
            return Error.Failure(
                code: ErrorCodes.UnexpectedMainDllCreation,
                description: "Unexpected failure during main DLL creation.",
                metadata: Error.ExceptionMetadata(exception)
            );
        }
    }
}