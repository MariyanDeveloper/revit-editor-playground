using RevitEditorPlayground.Functional;

namespace RevitEditorPlayground.Compilation.Utils;

public static class RawCodebases
{
    extension(RawCodebase)
    {
        public static Result<RawCodebase> FromRawCode(string[] code)
        {
            return new RawCodebase(Code: code);
        }

        public static Result<RawCodebase> FromRawCode(string code)
        {
            return new RawCodebase(Code: [code]);
        }

        public static Result<RawCodebase> FromCSharpFiles(string[] filePaths)
        {
            var codebase = filePaths
                .Select(CsharpFile.FromExistingFile)
                .Flatten()
                .Select(file => file.Content)
                .ToList();

            return new RawCodebase(Code: codebase);
        }

        public static Result<RawCodebase> FromSingleFile(string filePath)
        {
            return CsharpFile
                .FromExistingFile(filePath)
                .Map(csharpFile => new RawCodebase(Code: [csharpFile.Content]));
        }
    }
}
