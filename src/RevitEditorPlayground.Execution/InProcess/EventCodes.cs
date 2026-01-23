namespace RevitEditorPlayground.Execution.InProcess;

public static class EventCodes
{
    public static class Timeline
    {
        public const string CompilationStarted = "COMPILATION_STARTED";
        public const string CompiledCode = "COMPILED_CODE";
        public const string ExecutableCreationStarting = "EXECUTABLE_CREATION_STARTING";
        public const string ExecutableCreated = "EXECUTABLE_CREATED";
        public const string ProcessCreationStarting = "PROCESS_CREATION_STARTING";
        public const string ProcessStarted = "PROCESS_STARTED";
        public const string ProcessExited = "PROCESS_EXITED";
        public const string ExecutableDeleted = "EXECUTABLE_DELETED";
        
    }
}