using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using RevitEditorPlayground.Compilation;
using RevitEditorPlayground.Execution.Contracts;
using RevitEditorPlayground.Execution.Shared;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared;
using RevitEditorPlayground.Shared.Events;
using AssemblyName = RevitEditorPlayground.Compilation.AssemblyName;

namespace RevitEditorPlayground.Execution.InMemory;


public record InMemoryExecutionOptions(
    AssemblyName AssemblyName,
    FrameworkVersion Framework,
    LanguageVersion LanguageVersion,
    IReadOnlyList<CompilationDependency> Dependencies,
    AbsolutePath ScriptDirectory,
    Option<object?[]> Args
    );


public record Script(
    string Name,
    ExecutableScript Executable);

public delegate object? ExecutableScript(object?[] args);

public record ExecutedScript(
    string Name,
    Option<object?[]> Args,
    Option<object> Output);

public record ScriptMethod(MethodInfo Method, ScriptAttribute Attribute);

public record InMemoryExecutionOutput(
    CompiledCode CompiledCode,
    Assembly Assembly,
    ExecutedScript ExecutedScript,
    IReadOnlyList<DomainEvent> Events);

public record BinaryScript(AbsolutePath Directory, DllFile Main, IReadOnlyList<DllFile> Dependencies);