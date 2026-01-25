#if NET
using System.Reflection;
using System.Runtime.Loader;

namespace RevitEditorPlayground.Execution.InMemory.Utils;
public record AssemblyLoadedArgs(Assembly? Assembly, AssemblyName AssemblyName, string Path);

public class AssemblyLoadContext(string assemblyPath) : System.Runtime.Loader.AssemblyLoadContext(isCollectible: true)
{
    public event EventHandler<AssemblyLoadedArgs>? AssemblyLoaded;
    private readonly AssemblyDependencyResolver _resolver = new(assemblyPath);

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        
        if (assemblyPath is null)
        {
            return null;
        }
        
        var assembly = LoadFromAssemblyPath(assemblyPath);
        
        AssemblyLoaded?.Invoke(this, new AssemblyLoadedArgs(assembly, assemblyName, assemblyPath));
            
        return assembly;
    }
}
#endif