using Functional;

namespace RevitEditorPlayground.Compilation.Utils;

public static class AssemblyNames
{
    extension(AssemblyName)
    {
        public static Result<AssemblyName> FromString(string assemblyName)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                return Error.Failure(description: "Assembly name is empty");
            }

            return new AssemblyName(assemblyName);
        }
    }
}
