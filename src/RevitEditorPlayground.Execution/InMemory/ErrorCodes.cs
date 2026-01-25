namespace RevitEditorPlayground.Execution.InMemory;

public static class ErrorCodes
{
    public const string UnexpectedMainDllCreation = "UNEXPECTED_MAIN_DLL_CREATION";
    public const string UnexpectedDependencyDllCreation = "UNEXPECTED_DEPENDENCY_DLL_CREATION";
    public const string UnexpectedScriptResolution = "UNEXPECTED_SCRIPT_RESOLUTION";
    public const string UnexpectedInMemoryExecutionFailure = "UNEXPECTED_IN_MEMORY_EXECUTION_FAILURE";
    public const string UnexpectedScriptExecutionFailure = "UNEXPECTED_SCRIPT_EXECUTION_FAILURE";
    public const string MultipleScriptsFound = "MULTIPLE_SCRIPTS_FOUND";
    public const string NoScriptsFound = "NO_SCRIPTS_FOUND";
}