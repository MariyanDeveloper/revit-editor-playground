using RevitEditorPlayground.Execution.InMemory;
using RevitEditorPlayground.Functional;

namespace RevitEditorPlayground.Execution.Tests.InMemory.Utils;

public static class Snapshots
{
    extension(IEnumerable<Result<InMemoryExecutionOutput>> results)
    {
        public object SnapshotNormalized()
        {
            var outputs = results.ToArray();
            var successCount = outputs.Count(result => result.IsValid);
            var failureCount = outputs.Count(result => !result.IsValid);
            var tests = outputs.Select(r => r.SnapshotNormalize())
                .ToArray();
            
            return new
            {
                SuccessCount = successCount,
                FailureCount = failureCount,
                Tests = tests
            };
        }
    }
    
    extension(Result<InMemoryExecutionOutput> result)
    {
        public object SnapshotNormalize()
        {

            if (!result.IsValid)
            {
                return new
                {
                    result.Error
                };
            }
            
            var value = result.Value!;

            return new
            {
                CompiledCode = new
                {
                    BytesCount = value.CompiledCode.Bytes.Count,
                    Compilation = new
                    {
                        value.CompiledCode.Compilation.AssemblyName, value.CompiledCode.Compilation.LanguageVersion
                    }
                },
                Assembly = new
                {
                    value.Assembly.FullName
                },
                ExecutedScript = new
                {
                    Args = value.ExecutedScript.Args.InternalValue(),
                    Output = value.ExecutedScript.Output.InternalValue(),
                    value.ExecutedScript.Name
                },
                value.Events
            };
        }
    }
}