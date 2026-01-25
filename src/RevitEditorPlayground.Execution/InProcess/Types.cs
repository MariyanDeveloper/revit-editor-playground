using Microsoft.CodeAnalysis.CSharp;
using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Execution.InMemory;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared;
using RevitEditorPlayground.Shared.Events;

namespace RevitEditorPlayground.Execution.InProcess;

public record PhysicalExecutable(
    IReadOnlyList<byte> Bytes,
    AbsolutePath Path);

public record InProcessExecutionOptions(
    AssemblyName AssemblyName,
    FrameworkVersion Framework,
    LanguageVersion LanguageVersion,
    IReadOnlyList<CompilationDependency> Dependencies,
    Option<AbsolutePath> ExecutablePath
    );


public record InProcessExecutionOutput(
    CompiledCode CompiledCode,
    string StandardOutput,
    string StandardError,
    IReadOnlyList<DomainEvent> Events);
