using Microsoft.CodeAnalysis.CSharp;
using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Execution.Shared;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared;
using RevitEditorPlayground.Shared.Events;

namespace RevitEditorPlayground.Execution.InProcess;


public record Executable(
    AbsolutePath Path,
    IReadOnlyList<byte> Bytes);


public record ExecutableBundle(
    Executable Executable,
    IReadOnlyList<DllFile> Dependencies,
    AbsolutePath WorkingDirectory);

public record ExecutableName(string Value) : TypeAlias<string>(Value);

public record InProcessExecutionOptions(
    AssemblyName AssemblyName,
    FrameworkVersion Framework,
    LanguageVersion LanguageVersion,
    IReadOnlyList<CompilationDependency> Dependencies,
    AbsolutePath WorkingDirectory,
    Option<ExecutableName> ExecutableName);


public record ExecutedProcess(
    string Name,
    int Id,
    string StandardOutput,
    string StandardError,
    int ExitCode
    );

public record InProcessExecutionOutput(
    CompiledCode CompiledCode,
    ExecutedProcess ExecutedProcess,
    IReadOnlyList<Error> Errors,
    IReadOnlyList<DomainEvent> Events);
