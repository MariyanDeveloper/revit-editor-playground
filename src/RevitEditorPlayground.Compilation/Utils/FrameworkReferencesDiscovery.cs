using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Functional;
using Microsoft.CodeAnalysis;
using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Compilation.Utils;

public static class FrameworkReferencesDiscovery
{
    extension(Framework)
    {
        public static Result<Framework> Net48()
        {
            var frameworkDirectory =
                @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8";

            return AbsolutePath.FromExistingDirectory(
                    frameworkDirectory
                )
                .Map(path =>
                {
                    var references = Directory.EnumerateFiles(path, "*.dll", SearchOption.TopDirectoryOnly)
                        .Where(IsManagedAssembly)
                        .Select(file => MetadataReference.CreateFromFile(file))
                        .ToList();
                    
                    return new Framework(Version: FrameworkVersion.Net48, References: references);
                });
        }
        
        private static bool IsManagedAssembly(string path)
        {
            try
            {
                using var stream = File.OpenRead(path);
                using var pe = new PEReader(stream);

                if (!pe.HasMetadata) return false;

                // This ensures it’s CLR metadata, not just any metadata blob.
                var reader = pe.GetMetadataReader();
                return reader.IsAssembly;
            }
            catch
            {
                return false;
            }
        }

    }
}