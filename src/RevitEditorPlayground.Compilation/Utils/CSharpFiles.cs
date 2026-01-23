using RevitEditorPlayground.Functional;

namespace RevitEditorPlayground.Compilation.Utils;

public static class CSharpFiles
{
    private const string CsExtension = ".cs";

    extension(CsharpFile)
    {
        public static Result<CsharpFile> FromExistingFile(string filePath)
        {
            return Results
                .Ok(filePath)
                .Then(static filePath =>
                {
                    return Results.TryCatch(
                        func: () =>
                        {
                            if (!File.Exists(filePath))
                            {
                                return Error.Failure(
                                    description: $"File does not exist: {filePath}"
                                );
                            }

                            var extension = Path.GetExtension(filePath);

                            if (extension != CsExtension)
                            {
                                return Error.Failure(
                                    description: $"File is not a C# file: {filePath}"
                                );
                            }

                            var text = File.ReadAllText(path: filePath);

                            return Results.Ok(new CsharpFile(filePath, Content: text));
                        },
                        failure: exception =>
                            Error.Failure(
                                description: $"Failed to read file: {exception.Message}. {exception.StackTrace}"
                            )
                    );
                });
        }
    }
}
