using Functional;
using Microsoft.CodeAnalysis.CSharp;
using RevitEditorPlayground.Compilation.Utils;

namespace RevitEditorPlayground.Compilation;

public static class CompliedCodeCreation
{
    extension(CompiledCode)
    {
        public static Result<CompiledCode> FromCodebase(
            RawCodebase codebase,
            AssemblyName assemblyName,
            CompileOptions compileOptions
        )
        {
            try
            {
                var references = compileOptions.AllReferences();

                return CSharpSyntaxTree
                    .ParseText(
                        codebase: codebase.Code,
                        parseOptions: compileOptions.ParseOptions,
                        globalUsings: compileOptions.GlobalUsings
                    )
                    .Map(syntaxTrees =>
                        CSharpCompilation
                            .Create(
                                assemblyName: assemblyName,
                                syntaxTrees: syntaxTrees,
                                references: references,
                                options: compileOptions.CompilationOptions
                            )
                            .WithContext(syntaxTrees)
                    )
                    .Then(input =>
                    {
                        var compilation = input.Value;

                        return compilation
                            .EmitBytes(compileOptions.EmitOptions)
                            .WithContext(compilation);
                    })
                    .Map(input =>
                    {
                        var bytes = input.Value;
                        var compilation = input.Context;
                        return new CompiledCode(Bytes: bytes, Compilation: compilation);
                    });
            }
            catch (Exception e)
            {
                return Error.Failure(
                    description: $"Unexpected failure during compilation: {e.Message}. {e.StackTrace}",
                    metadata: new Dictionary<string, object>()
                    {
                        ["innerException"] = e.InnerException,
                    }
                );
            }
        }
    }
}
