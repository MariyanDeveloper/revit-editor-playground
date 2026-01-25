using RevitEditorPlayground.Functional;

namespace RevitEditorPlayground.Execution.InMemory.Utils;

public static class ScriptExecution
{
    extension(Script script)
    {
        public Result<ExecutedScript> Execute(Option<object?[]> args)
        {
            try
            {
                var argsArray = args.OrElse([]);
                var result = script.Executable.Invoke(argsArray);
                
                var output = Option.FromOptional(result);
                
                return new ExecutedScript(Name: script.Name, Args: args, Output: output );
            }
            catch (Exception e)
            {
                return Error.UnexpectedScriptExecutionFailure(e);
            }
        }
    }
}