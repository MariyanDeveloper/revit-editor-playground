using System.Reflection;

namespace RevitEditorPlayground.Execution.InMemory.Utils;

public static class ExecutableScriptsFactory
{
    extension(ExecutableScript)
    {
        public static ExecutableScript FromMethod(MethodInfo method)
        {
            ExecutableScript script = args =>
            {
                var parameters = method.GetParameters();
        
                if (!parameters.Any())
                {
                    return method.Invoke(obj: null, parameters: null);
                }
                
                return method.Invoke(obj: null, parameters: args);
            };
            
            return script;
        }
    }
}