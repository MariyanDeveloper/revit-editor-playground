using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Compilation;

public record CompiledCode(IReadOnlyList<byte> Bytes, CSharpCompilation Compilation);

public enum FrameworkVersion
{
    Net48,
}

public record Framework(FrameworkVersion Version, IReadOnlyList<MetadataReference> References);

public record CompileOptions(
    Framework Framework,
    IReadOnlyList<MetadataReference> Packages,
    CSharpCompilationOptions CompilationOptions,
    CSharpParseOptions ParseOptions,
    Option<EmitOptions> EmitOptions,
    IReadOnlyList<string> GlobalUsings
);

public record RawCodebase(IReadOnlyList<string> Code);

public record CsharpFile(string FilePath, string Content);

public record AssemblyName(string Value) : TypeAlias<string>(Value);

public record CompilationContext(
    RawCodebase Codebase,
    AssemblyName AssemblyName,
    CompileOptions CompileOptions
);
