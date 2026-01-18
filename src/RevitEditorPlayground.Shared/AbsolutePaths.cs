using Functional;

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
    extension(AbsolutePath)
    {
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
    }
}
