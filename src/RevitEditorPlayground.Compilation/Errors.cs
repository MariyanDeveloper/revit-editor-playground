using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared.Errors;

namespace RevitEditorPlayground.Compilation;

public static class ErrorCodes
{
    public const string UnexpectedCompilationFailure = "UNEXPECTED_COMPILATION_FAILURE";
    public const string UnexpectedSyntaxTreeParsing = "UNEXPECTED_SYNTAX_TREE_PARSING";
    public const string UnexpectedEmiting = "UNEXPECTED_EMITTING";
    public const string FailedCompilation = "FAILED_COMPILATION";
}

public record FailedDiagnostic(Diagnostic Diagnostic, string Message);

public static class Errors
{
    extension(Error)
    {
        public static Error FailedCompilation(EmitResult emitResult)
        {
            var diagnostics = emitResult
                .Diagnostics.Select(diagnostic =>
                {
                    var message = diagnostic.GetMessage();
                    return new FailedDiagnostic(diagnostic, message);
                })
                .ToList();

            return Error.Failure(
                code: ErrorCodes.FailedCompilation,
                description: "Failed compilation",
                metadata: new Dictionary<string, object> { ["diagnostics"] = diagnostics }
            );
        }

        public static Error UnexpectedSyntaxTreeParsing(Exception exception)
        {
            return Error.Failure(
                code: ErrorCodes.UnexpectedSyntaxTreeParsing,
                description: "Unexpected failure during syntax tree parsing.",
                metadata: Error.ExceptionMetadata(exception)
            );
        }

        public static Error UnexpectedEmiting(Exception exception)
        {
            return Error.Failure(
                code: ErrorCodes.UnexpectedEmiting,
                description: "Unexpected failure during emmiting",
                metadata: Error.ExceptionMetadata(exception)
            );
        }

        public static Error UnexpectedCompilationFailure(Exception exception)
        {
            return Error.Failure(
                code: ErrorCodes.UnexpectedCompilationFailure,
                description: "Unexpected failure during compilation.",
                metadata: Error.ExceptionMetadata(exception)
            );
        }
    }
}
