using Functional;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace RevitEditorPlayground.Compilation.Utils;

public static class CompileOptionsByFrameworkReferences
{
    extension(CompileOptions options)
    {
        public IReadOnlyList<MetadataReference> AllReferences()
        {
            return [.. options.Framework.References, .. options.Packages];
        }
    }
}

public static class CompileOptionsByFrameworkFactories
{
    extension(CompileOptions)
    {
        public static Result<CompileOptions> FromFrameworkVersion(
            FrameworkVersion frameworkVersion,
            LanguageVersion languageVersion = LanguageVersion.Latest,
            OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary,
            EmitOptions? emitOptions = null,
            IReadOnlyList<MetadataReference>? packages = null,
            IReadOnlyList<string>? globalUsings = null
        )
        {
            var discoveredFramework = frameworkVersion switch
            {
                FrameworkVersion.Net48 => Framework.Net48(),
                _ => throw new ArgumentOutOfRangeException(
                    nameof(frameworkVersion),
                    frameworkVersion,
                    null
                ),
            };

            return discoveredFramework.Map(framework =>
                CompileOptions.From(
                    framework: framework,
                    languageVersion: languageVersion,
                    outputKind: outputKind,
                    emitOptions: emitOptions,
                    packages: packages,
                    globalUsings: globalUsings
                )
            );
        }

        public static CompileOptions From(
            Framework framework,
            LanguageVersion languageVersion = LanguageVersion.Latest,
            OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary,
            EmitOptions? emitOptions = null,
            IReadOnlyList<MetadataReference>? packages = null,
            IReadOnlyList<string>? globalUsings = null
        )
        {
            return new CompileOptions(
                Framework: framework,
                Packages: packages ?? [],
                CompilationOptions: new CSharpCompilationOptions(outputKind: outputKind),
                ParseOptions: new CSharpParseOptions(languageVersion: languageVersion),
                EmitOptions: Option.FromOptional(emitOptions),
                GlobalUsings: globalUsings ?? []
            );
        }
    }
}
