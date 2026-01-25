using System.Reflection;
using RevitEditorPlayground.Execution.Contracts;
using RevitEditorPlayground.Functional;

namespace RevitEditorPlayground.Execution.InMemory.Utils;

public static class ScriptsDiscovery
{
    extension(Script)
    {
        public static Result<Script> FromAssembly(Assembly assembly)
        {
            try
            {
                var scriptMethods = assembly
                    .GetTypes()
                    .SelectMany(type =>
                    {
                        return type.GetMethods()
                            .SelectMany(method =>
                            {
                                if (!method.IsStatic)
                                {
                                    return Enumerable.Empty<ScriptMethod>();
                                }

                                var scriptAttribute = method.GetCustomAttributes()
                                    .OfType<ScriptAttribute>()
                                    .SingleOrDefault();

                                if (scriptAttribute is null)
                                {
                                    return Enumerable.Empty<ScriptMethod>();
                                }

                                var output = new ScriptMethod(Method: method, Attribute: scriptAttribute);

                                return [output];
                            });
                    })
                    .ToList();

                if (!scriptMethods.Any())
                {
                    return Error.NoScriptsFound();
                }

                if (scriptMethods.Count > 1)
                {
                    return Error.MultipleScriptsFound(scriptMethods);
                }

                var scriptMethod = scriptMethods.Single();

                var executableScript = ExecutableScript.FromMethod(scriptMethod.Method);
                var script = new Script(Name: scriptMethod.Attribute.Name, Executable: executableScript);

                return script;
            }
            catch (Exception e)
            {
                return Error.UnexpectedScriptResolution(e);
            }
        }
    }
}