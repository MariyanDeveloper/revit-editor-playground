using System.Diagnostics;
using RevitEditorPlayground.Functional;

namespace RevitEditorPlayground.Execution.InProcess.Utils;

public static class ProcessExecution
{
    extension(ExecutedProcess)
    {
        public static Result<ExecutedProcess> RunFromBundle(ExecutableBundle bundle)
        {
            return Process.StartExecutable(bundle.Executable)
                .Then(startedProcess =>
                {
                    var processId = startedProcess.Id;
                    var processName = startedProcess.ProcessName;

                    return startedProcess.AwaitExit()
                        .WithContext((processId, processName));
                })
                .Map(input =>
                {
                    var (processId, processName) = input.Context;
                    var process = input.Value;
                    var stdout = process.StandardOutput.ReadToEnd();
                    var stderr = process.StandardError.ReadToEnd();
                    
                    return new ExecutedProcess(
                        Id: processId,
                        Name: processName,
                        ExitCode: process.ExitCode,
                        StandardOutput: stdout,
                        StandardError: stderr
                    );
                });
        }
    }
}