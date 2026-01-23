using Microsoft.CodeAnalysis.CSharp;
using RevitEditorPlayground.Functional;

namespace RevitEditorPlayground.Compilation.Utils;

public static class CompilationContexts
{
    extension(CompilationContext)
    {
        public static Result<CompilationContext> FromCSharpFiles(
            string[] filePaths,
            string assemblyName,
            FrameworkVersion frameworkVersion,
            LanguageVersion languageVersion = LanguageVersion.CSharp14
        )
        {
            return CompileOptions
                .FromFrameworkVersion(
                    frameworkVersion: frameworkVersion,
                    languageVersion: languageVersion
                )
                .ThenWithContext(_ => RawCodebase.FromCSharpFiles(filePaths))
                .ThenWithContext(_ => AssemblyName.FromString(assemblyName))
                .Map(static input =>
                {
                    var assemblyName = input.Value;
                    var codebase = input.Context.Value;
                    var compileOptions = input.Context.Context;

                    return new CompilationContext(codebase, assemblyName, compileOptions);
                });
        }

        public static Result<CompilationContext> FromRawCode(
            string[] code,
            string assemblyName,
            FrameworkVersion frameworkVersion,
            LanguageVersion languageVersion = LanguageVersion.CSharp14
        )
        {
            return CompileOptions
                .FromFrameworkVersion(
                    frameworkVersion: frameworkVersion,
                    languageVersion: languageVersion
                )
                .ThenWithContext(_ => RawCodebase.FromRawCode(code))
                .ThenWithContext(_ => AssemblyName.FromString(assemblyName))
                .Map(static input =>
                {
                    var assemblyName = input.Value;
                    var codebase = input.Context.Value;
                    var compileOptions = input.Context.Context;

                    return new CompilationContext(codebase, assemblyName, compileOptions);
                });
        }

        public static Result<CompilationContext> FromCSharpFile(
            string csharpFilePath,
            string assemblyName,
            FrameworkVersion frameworkVersion,
            LanguageVersion languageVersion = LanguageVersion.CSharp14
        )
        {
            return CompileOptions
                .FromFrameworkVersion(
                    frameworkVersion: frameworkVersion,
                    languageVersion: languageVersion
                )
                .ThenWithContext(_ => RawCodebase.FromSingleFile(csharpFilePath))
                .ThenWithContext(_ => AssemblyName.FromString(assemblyName))
                .Map(static input =>
                {
                    var assemblyName = input.Value;
                    var codebase = input.Context.Value;
                    var compileOptions = input.Context.Context;

                    return new CompilationContext(codebase, assemblyName, compileOptions);
                });
        }
    }
}
