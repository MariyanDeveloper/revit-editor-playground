using RevitEditorPlayground.Functional;

namespace RevitEditorPlayground.Shared;

public abstract record TypeAlias<T>(T Value)
{
    public static implicit operator T(TypeAlias<T> alias)
    {
        return alias.Value;
    }
};

public record AbsolutePath(string Value) : TypeAlias<string>(Value);

public static class AbsolutePaths
{
    extension(AbsolutePath absolutePath)
    {
        public static AbsolutePath FromKnownFile(string filePath)
        {
            return new AbsolutePath(filePath);
        }
        
        public static Result<AbsolutePath> FromExistingFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return Error.Failure(description: $"File does not exist: {filePath}");
            }

            return new AbsolutePath(filePath);
        }
        
        public static Result<AbsolutePath> FromExistingDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return Error.Failure(description: $"Directory does not exist: {directory}");
            }

            return new AbsolutePath(directory);
        }

        public static AbsolutePath FromCurrentDirectory(string fileName)
        {
            return new AbsolutePath(Path.Combine(Directory.GetCurrentDirectory(), fileName));
        }

        public AbsolutePath Combine(string fileName)
        {
            var path = Path.Combine(absolutePath, fileName);
            
            return new AbsolutePath(path);
        }
        
    }
}
