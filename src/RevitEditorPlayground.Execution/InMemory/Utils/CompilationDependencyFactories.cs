using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Execution.InMemory.Utils;

public static class CompilationDependencyFactories
{
    extension(CompilationDependency)
    {
        public static CompilationDependency FromPath(AbsolutePath path, DependencyKind kind)
        {
            return new CompilationDependency(Path: path, Kind: kind);
        }
        
        public static CompilationDependency CopyLocal(AbsolutePath path)
        {
            return CompilationDependency.FromPath(path, DependencyKind.CopyLocal);
        }
        
        public static CompilationDependency ProvidedByHost(AbsolutePath path)
        {
            return CompilationDependency.FromPath(path, DependencyKind.ProvidedByHost);
        }
    }
}