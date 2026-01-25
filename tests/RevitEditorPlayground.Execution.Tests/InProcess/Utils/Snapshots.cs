using RevitEditorPlayground.Execution.InProcess;
using RevitEditorPlayground.Functional;

namespace RevitEditorPlayground.Execution.Tests.InProcess.Utils;

public static class Snapshots
{
    extension(Result<InProcessExecutionOutput> output)
    {
        public object SnapshotNormalized()
        {
            if (!output.IsValid)
            {
                return new
                {
                    output.Error
                };
            }
            
            var value = output.Value!;
            
            var executedProcess = value.ExecutedProcess;
            
            return new
            {
                Process = new
                {
                    executedProcess.Name,
                    executedProcess.StandardOutput,
                    executedProcess.StandardError,
                    executedProcess.ExitCode
                },
                Errors = value.Errors.Select(error => new
                {
                    error.Code,
                    error.Description
                }),
                Events = value.Events.Select(e => new
                {
                    e.Message,
                    e.Code,
                    e.Level
                }).ToList()
            };
        }
    }

}