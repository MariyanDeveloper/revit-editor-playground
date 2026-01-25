using System.Diagnostics;
using RevitEditorPlayground.Functional;

namespace RevitEditorPlayground.Execution.InProcess.Utils;

public static class ProcessSafeWait
{
    extension(Process process)
    {
        public Result<Process> AwaitExit()
        {
            try
            {
                process.WaitForExit();
                
                return process;
            }
            catch (Exception e)
            {
                return Error.UnexpectedProcessFailure(e);
            }
        }
    }
}