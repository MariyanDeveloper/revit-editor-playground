using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared;
using RevitEditorPlayground.Shared.Events;

namespace RevitEditorPlayground.Execution.InProcess;

public record PhysicalExecutable(
    IReadOnlyList<byte> Bytes,
    AbsolutePath Path);

public record InProcessExecutionContext(
    Option<AbsolutePath> ExecutablePath,
    RawCodebase RawCodebase,
    AssemblyName AssemblyName,
    CompileOptions CompileOptions
    );

public record InProcessExecutionOutput(
    CompiledCode CompiledCode,
    string StandardOutput,
    string StandardError,
    IReadOnlyList<DomainEvent> Events);
