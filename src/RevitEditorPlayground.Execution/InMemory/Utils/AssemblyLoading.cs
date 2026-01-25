using System.Reflection;
using RevitEditorPlayground.Functional;

namespace RevitEditorPlayground.Execution.InMemory.Utils;

public static class AssemblyLoading
{
    extension(Assembly)
    {
        public static Result<Assembly> LoadPlugin(string pluginPath)
        {
#if NETFRAMEWORK
            return Assembly.LoadPluginFramework(pluginPath);
#elif NET
            return Assembly.LoadPluginCore(pluginPath);
#else
            return Error.Failure(description: "Unsupported platform");
#endif
        }

#if NETFRAMEWORK
        private static Result<Assembly> LoadPluginFramework(string pluginPath)
        {
            try
            {
                var assembly = Assembly.LoadFrom(pluginPath);
                
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (assembly is null)
                {
                    return Error.Failure(description: $"Failed to load assembly: {pluginPath}");
                }
                
                return assembly;
            }
            catch (Exception e)
            {
                return Error.Failure(description: $"Failed to load assembly: {e.Message}. {e.StackTrace}");
            }
        }
#endif

#if NET
        private static Result<Assembly> LoadPluginCore(string pluginPath)
        {
            try
            {
                var loadContext = new AssemblyLoadContext(pluginPath);
                var assembly = loadContext.LoadFromAssemblyPath(pluginPath);
                
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (assembly is null)
                {
                    return Error.Failure(description: $"Failed to load assembly: {pluginPath}");
                }
                
                return assembly;
            }
            catch (Exception e)
            {
                return Error.Failure(description: $"Failed to load assembly: {e.Message}. {e.StackTrace}");
            }
        }
#endif
    }
}