using Meziantou.Framework.InlineSnapshotTesting;
using RevitEditorPlayground.Execution.InProcess;
using RevitEditorPlayground.Execution.Shared;
using RevitEditorPlayground.Execution.Tests.InProcess.Utils;
using RevitEditorPlayground.Execution.Tests.Utils;
using RevitEditorPlayground.Shared;

namespace RevitEditorPlayground.Execution.Tests.InProcess;

public class InProcessExecutionTests
{

  [Fact]
  public void ShouldRunCodeWithCopyLocalDependency()
  {
    var codebase = """
                   using System;
                   using System.Linq;
                   using System.Collections.Generic;
                   using Newtonsoft.Json;

                   var sample = new
                   {
                     Value = 5
                   };
                   var json = JsonConvert.SerializeObject(sample);
                   Console.WriteLine(json);
                   """;
    
    var output = InProcessExecution.RunTest(
      code: codebase,
      dependencies: List.Of(
        CompilationDependency.CopyLocal(
          path: AbsolutePath.FromExistingFile(InProcessLibs.NewtonsoftJson).ShouldHaveValue()
        )
      ));

    var normalized = output.SnapshotNormalized();

    InlineSnapshot.Validate(normalized, """
      {
        "process": {
          "name": "test_app",
          "standardOutput": "{\u0022Value\u0022:5}\r\n",
          "standardError": "",
          "exitCode": 0
        },
        "errors": [],
        "events": [
          {
            "message": "Compilation started",
            "code": "COMPILATION_STARTED",
            "level": "info"
          },
          {
            "message": "Compilation ended",
            "code": "COMPILATION_ENDED",
            "level": "info"
          },
          {
            "message": "Executable creation starting",
            "code": "EXECUTABLE_BUNDLE_CREATION_STARTED",
            "level": "info"
          },
          {
            "message": "Executable bundle created",
            "code": "EXECUTABLE_BUNDLE_CREATED",
            "level": "info"
          },
          {
            "message": "Bundle execution ended",
            "code": "BUNDLE_EXECUTION_ENDED",
            "level": "info"
          },
          {
            "message": "Executable bundle deleted",
            "code": "EXECUTABLE_BUNDLE_DELETED",
            "level": "info"
          }
        ]
      }
      """);
  }
  
  [Fact]
  public void ShouldHaveErrorInStandardError_WhenHasMissingRequiredRuntimeDependency()
  {
    var codebase = """
                   using System;
                   using System.Linq;
                   using System.Collections.Generic;
                   using Newtonsoft.Json;
                   
                   var sample = new
                   {
                     Value = 5
                   };
                   var json = JsonConvert.SerializeObject(sample);
                   Console.WriteLine(json);
                   """;
    
      var output = InProcessExecution.RunTest(
        code: codebase,
        dependencies: List.Of(
          CompilationDependency.ProvidedByHost(
            path: AbsolutePath.FromExistingFile(InProcessLibs.NewtonsoftJson).ShouldHaveValue()
            )
          ));

      var normalized = output.SnapshotNormalized();
      
      InlineSnapshot.Validate(normalized, """
        {
          "process": {
            "name": "test_app",
            "standardOutput": "",
            "standardError": "\nUnhandled Exception: System.IO.FileNotFoundException: Could not load file or assembly \u0027Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed\u0027 or one of its dependencies. The system cannot find the file specified.\r\n   at Program.\u003CMain\u003E$(String[] args)\n",
            "exitCode": -532462766
          },
          "errors": [],
          "events": [
            {
              "message": "Compilation started",
              "code": "COMPILATION_STARTED",
              "level": "info"
            },
            {
              "message": "Compilation ended",
              "code": "COMPILATION_ENDED",
              "level": "info"
            },
            {
              "message": "Executable creation starting",
              "code": "EXECUTABLE_BUNDLE_CREATION_STARTED",
              "level": "info"
            },
            {
              "message": "Executable bundle created",
              "code": "EXECUTABLE_BUNDLE_CREATED",
              "level": "info"
            },
            {
              "message": "Bundle execution ended",
              "code": "BUNDLE_EXECUTION_ENDED",
              "level": "info"
            },
            {
              "message": "Executable bundle deleted",
              "code": "EXECUTABLE_BUNDLE_DELETED",
              "level": "info"
            }
          ]
        }
        """);
  }
  
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

        var output = InProcessExecution.RunTest(codebase);
        
        var normalized = output.SnapshotNormalized();
        
        InlineSnapshot.Validate(normalized, """
          {
            "process": {
              "name": "test_app",
              "standardOutput": "Summary: -1;-2;-3\r\n",
              "standardError": "Opps. Something is wrong",
              "exitCode": 0
            },
            "errors": [],
            "events": [
              {
                "message": "Compilation started",
                "code": "COMPILATION_STARTED",
                "level": "info"
              },
              {
                "message": "Compilation ended",
                "code": "COMPILATION_ENDED",
                "level": "info"
              },
              {
                "message": "Executable creation starting",
                "code": "EXECUTABLE_BUNDLE_CREATION_STARTED",
                "level": "info"
              },
              {
                "message": "Executable bundle created",
                "code": "EXECUTABLE_BUNDLE_CREATED",
                "level": "info"
              },
              {
                "message": "Bundle execution ended",
                "code": "BUNDLE_EXECUTION_ENDED",
                "level": "info"
              },
              {
                "message": "Executable bundle deleted",
                "code": "EXECUTABLE_BUNDLE_DELETED",
                "level": "info"
              }
            ]
          }
          """);
        
    }
}
