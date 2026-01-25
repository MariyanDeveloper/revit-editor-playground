using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Execution.Shared;

public static class CompilationDependencies
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