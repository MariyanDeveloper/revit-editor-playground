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
                        CompiledCode.From(bytes: input.Value, compilation: input.Context)
                    );
            }
            catch (Exception e)
            {
                return Error.UnexpectedCompilationFailure(e);
            }
        }

        private static CompiledCode From(IReadOnlyList<byte> bytes, CSharpCompilation compilation)
        {
            return new CompiledCode(Bytes: bytes, Compilation: compilation);
        }
    }
}
