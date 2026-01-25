using System.Diagnostics;
using RevitEditorPlayground.Functional;

namespace RevitEditorPlayground.Execution.InProcess.Utils;

public static class Processes
{
    extension(Process)
    {
        public static Result<Process> StartExecutable(Executable executable)
        {
            try
            {
                var startedProcess =  Process.Start(startInfo: new ProcessStartInfo
                {
                    FileName = executable.Path,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                });

                if (startedProcess is null)
                {
                    return Error.FailedToStartProcess(executable);
                }
                
                return startedProcess;
            }
            catch (Exception e)
            {
                return Error.UnexpectedFailureToStartProcess(e, executable);
            }
        }
    }
}