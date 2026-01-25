namespace RevitEditorPlayground.Execution.InProcess;

public static class EventCodes
{
    public static class Timeline
    {
        public const string CompilationStarted = "COMPILATION_STARTED";
        public const string CompilationEnded = "COMPILATION_ENDED";
        
        public const string ExecutableBundleCreationStarting = "EXECUTABLE_BUNDLE_CREATION_STARTED";
        public const string ExecutableBundleCreated = "EXECUTABLE_BUNDLE_CREATED";
        
        public const string BundleExecutionEnded = "BUNDLE_EXECUTION_ENDED";
        public const string ExecutableBundleDeleted = "EXECUTABLE_BUNDLE_DELETED";
        
    }
}