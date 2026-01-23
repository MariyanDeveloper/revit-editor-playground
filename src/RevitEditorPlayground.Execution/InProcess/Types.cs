using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Shared;
using RevitEditorPlayground.Shared.Events;

namespace RevitEditorPlayground.Execution.InProcess;

public record PhysicalExecutable(
    IReadOnlyList<byte> Bytes,
    AbsolutePath Path);

public record InProcessExecutionOutput(
    CompiledCode CompiledCode,
    string StandardOutput,
    string StandardError,
    IReadOnlyList<DomainEvent> Events);
