using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Shared.Events;

namespace RevitEditorPlayground.Execution.InProcess;

public static class Events
{
    extension(DomainEvent)
    {
        public static DomainEvent ExecutableDeleted(PhysicalExecutable physicalExecutable)
        {
            return DomainEvent.Timeline(
                code: EventCodes.Timeline.ExecutableDeleted,
                message: "Executable deleted",
                metadata: new Dictionary<string, object>()
                {
                    ["fileName"] = physicalExecutable.Path.Value
                }
            );
        }
        public static DomainEvent ProcessExited(System.Diagnostics.Process process)
        {
            return DomainEvent.Timeline(
                code: EventCodes.Timeline.ProcessExited,
                message: "Process exited",
                metadata: new Dictionary<string, object>()
                {
                    ["processId"] = process.Id,
                    ["processName"] = process.ProcessName,
                    ["exitCode"] = process.ExitCode
                }
            );
        }

        public static DomainEvent ProcessStarted(System.Diagnostics.Process process)
        {
            return DomainEvent.Timeline(
                code: EventCodes.Timeline.ProcessStarted,
                message: "Process started",
                metadata: new Dictionary<string, object>()
                {
                    ["processId"] = process.Id,
                    ["processName"] = process.ProcessName
                }
            );
        }

        public static DomainEvent StartingProcessCreation(PhysicalExecutable physicalExecutable)
        {
            return DomainEvent.Timeline(
                code: EventCodes.Timeline.ProcessCreationStarting,
                message: "Starting process creation",
                metadata: new Dictionary<string, object>()
                {
                    ["fileName"] = physicalExecutable.Path.Value
                }
            );
        }

        public static DomainEvent CreatedExecutable(PhysicalExecutable physicalExecutable)
        {
            return DomainEvent.Timeline(
                code: EventCodes.Timeline.ExecutableCreated,
                message: "Executable created",
                metadata: new Dictionary<string, object>()
                {
                    ["fileName"] = physicalExecutable.Path.Value
                }
            );
        }

        public static DomainEvent StartingExecutableCreation()
        {
            return DomainEvent.Timeline(
                code: EventCodes.Timeline.ExecutableCreationStarting,
                message: "Starting executable creation"
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

        public static DomainEvent CompiledCode(
            CompiledCode compiledCode)
        {
            return DomainEvent.Timeline(
                code: EventCodes.Timeline.CompiledCode,
                message: "Compiled code",
                metadata: new Dictionary<string, object>()
                {
                    ["compiledCode"] = compiledCode
                }
            );
        }
    }
}
