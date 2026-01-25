using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Execution.Shared;

public enum DependencyKind
{
    ProvidedByHost,
    CopyLocal
}

public record CompilationDependency(
    AbsolutePath Path,
    DependencyKind Kind
);

public record DllFile(string Value) : TypeAlias<string>(Value);
