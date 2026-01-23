using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared.Errors;

namespace RevitEditorPlayground.Execution.InProcess;

public static class ErrorCodes
{
    public const string UnexpectedProcessFailure = "UNEXPECTED_PROCESS_FAILURE";
    public const string UnexpectedExecutableCreation = "UNEXPECTED_EXECUTABLE_CREATION";
    public const string UnexpectedExecutableDeletion = "UNEXPECTED_EXECUTABLE_DELETION";
    public const string FailedToStartProcess = "FAILED_TO_START_PROCESS";
    public const string UnexpectedFailureToStartProcess = "UNEXPECTED_FAILURE_TO_START_PROCESS";
}

public static class Errors
{
    extension(Error)
    {
        public static Error UnexpectedFailureToStartProcess(Exception exception, PhysicalExecutable physicalExecutable)
        {
            var metadata = Error.ExceptionMetadata(exception);
            metadata.Add("fileName", physicalExecutable.Path.Value);
            
            return Error.Failure(
                code: ErrorCodes.UnexpectedFailureToStartProcess,
                description: "Unexpected failure to start process.",
                metadata: metadata
            );
        }
        
        public static Error FailedToStartProcess(PhysicalExecutable physicalExecutable)
        {
            return Error.Failure(
                code: ErrorCodes.FailedToStartProcess,
                description: "Failed to start process",
                metadata: new Dictionary<string, object>()
                {
                    ["fileName"] = physicalExecutable.Path.Value
                }
            );
        }
        
        public static Error UnexpectedExecutableDeletion(Exception exception)
        {
            return Error.Failure(
                code: ErrorCodes.UnexpectedExecutableDeletion,
                description: "Unexpected failure during executable deletion.",
                metadata: Error.ExceptionMetadata(exception)
            );
        }
        
        public static Error UnexpectedExecutableCreation(Exception exception)
        {
            return Error.Failure(
                code: ErrorCodes.UnexpectedExecutableCreation,
                description: "Unexpected failure during executable creation.",
                metadata: Error.ExceptionMetadata(exception)
            );
        }
        
        public static Error UnexpectedProcessFailure(Exception exception)
        {
            return Error.Failure(
                code: ErrorCodes.UnexpectedProcessFailure,
                description: "Unexpected failure during process execution.",
                metadata: Error.ExceptionMetadata(exception)
            );
        }
    }
}