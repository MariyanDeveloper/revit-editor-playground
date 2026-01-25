using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Shared.Events;

namespace RevitEditorPlayground.Execution.InProcess;

public static class Events
{
    extension(DomainEvent)
    {
        public static DomainEvent ExecutableBundleDeleted(ExecutableBundle executableBundle)
        {
            return DomainEvent.Timeline(
                code: EventCodes.Timeline.ExecutableBundleDeleted,
                message: "Executable bundle deleted",
                metadata: new Dictionary<string, object>()
                {
                    ["fileName"] = executableBundle.WorkingDirectory.Value
                }
            );
        }
        public static DomainEvent BundleExecutionEnded(ExecutedProcess process)
        {
            return DomainEvent.Timeline(
                code: EventCodes.Timeline.BundleExecutionEnded,
                message: "Bundle execution ended",
                metadata: new Dictionary<string, object>()
                {
                    ["processId"] = process.Id,
                    ["processName"] = process.Name,
                    ["exitCode"] = process.ExitCode
                }
            );
        }

        public static DomainEvent CreatedExecutableBundle(ExecutableBundle executableBundle)
        {
            return DomainEvent.Timeline(
                code: EventCodes.Timeline.ExecutableBundleCreated,
                message: "Executable bundle created",
                metadata: new Dictionary<string, object>()
                {
                    ["fileName"] = executableBundle.WorkingDirectory.Value
                }
            );
        }

        public static DomainEvent StartingExecutableBundleCreation()
        {
            return DomainEvent.Timeline(
                code: EventCodes.Timeline.ExecutableBundleCreationStarting,
                message: "Executable creation starting"
            );
        }
        public static DomainEvent CompilationStarted(CompileOptions compileOptions, string assemblyName)
        {
            return DomainEvent.Timeline(
                code: EventCodes.Timeline.CompilationStarted,
                message: "Compilation started",
                metadata: new Dictionary<string, object>()
                {
                    ["compileOptions"] = compileOptions,
                    ["assemblyName"] = assemblyName
                }
            );
        }

        public static DomainEvent CompilationEnded(
            CompiledCode compiledCode)
        {
            return DomainEvent.Timeline(
                code: EventCodes.Timeline.CompilationEnded,
                message: "Compilation ended",
                metadata: new Dictionary<string, object>()
                {
                    ["compiledCode"] = compiledCode
                }
            );
        }
    }
}
