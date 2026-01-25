using Meziantou.Framework.InlineSnapshotTesting;
using RevitEditorPlayground.Execution.InProcess;

namespace RevitEditorPlayground.Execution.Tests.InProcess;

public class InProcessExecutionTests
{
    [Fact]
    public void ShouldExecuteCodeInSeparateProcess()
    {
        var codebase = """
                       using System;
                       using System.Linq;
                       using System.Collections.Generic;
                       var items = new List<string>()
                       {
                           "1", "2", "3"
                       };
                       
                       var transformedItems = items
                           .Select(item => $"-{item}");
                       
                       var formattedText = string.Join(";", transformedItems);
                       
                       Console.WriteLine($"Summary: {formattedText}");
                       Console.Error.Write("Opps. Something is wrong");
                       """;

        var output = InProcessExecution.RunRawCode(codebase);
        
        var normalizedOutput = output.SnapshotNormalized();
        
        InlineSnapshot.Validate(normalizedOutput, """
            {
              "standardError": "Opps. Something is wrong",
              "standardOutput": "Summary: -1;-2;-3\r\n",
              "events": [
                {
                  "message": "Compilation started",
                  "code": "COMPILATION_STARTED",
                  "level": "info"
                },
                {
                  "message": "Compiled code",
                  "code": "COMPILED_CODE",
                  "level": "info"
                },
                {
                  "message": "Starting executable creation",
                  "code": "EXECUTABLE_CREATION_STARTING",
                  "level": "info"
                },
                {
                  "message": "Executable created",
                  "code": "EXECUTABLE_CREATED",
                  "level": "info"
                },
                {
                  "message": "Process started",
                  "code": "PROCESS_STARTED",
                  "level": "info"
                },
                {
                  "message": "Process exited",
                  "code": "PROCESS_EXITED",
                  "level": "info"
                },
                {
                  "message": "Executable deleted",
                  "code": "EXECUTABLE_DELETED",
                  "level": "info"
                }
              ]
            }
            """);
        
    }
}
