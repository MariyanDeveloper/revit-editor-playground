using Meziantou.Framework.InlineSnapshotTesting;
using RevitEditorPlayground.Execution.InMemory;
using RevitEditorPlayground.Execution.Tests.InMemory.Utils;
using RevitEditorPlayground.Execution.Tests.Utils;

namespace RevitEditorPlayground.Execution.Tests.InMemory;


public class InMemoryExecutionTests
{
    [Fact]
    public void ShouldReturnErrorWhenNoScriptsAreFound()
    {
        var codebase = """
                       using System;
                       using RevitEditorPlayground.Execution.Contracts;

                       public class Functions
                       {
                        public static double DoThing(double value, string[] args)
                        {
                            Console.WriteLine($"Value: {value}");
                            
                            foreach (var arg in args)
                            {
                                Console.WriteLine($"Arg: {arg}");
                            }

                            return value;
                        }
                       }
                       """;
        
        var result = InMemoryExecution.RunTest(
            codebase: codebase,
            args: [10, Array.Of("1", "2")],
            assemblyName: "Script1"
        );
        
        var normalizedResult = result.SnapshotNormalize();
        
        InlineSnapshot.Validate(normalizedResult, """
            {
              "error": {
                "code": "NO_SCRIPTS_FOUND",
                "description": "No scripts found",
                "metadata": null,
                "type": "failure",
                "innerErrors": []
              }
            }
            """);
    }

    [Fact]
    public void ShouldReturnErrorWhenMultipleScriptsAreFound()
    {
        var codebase = """
                       using System;
                       using RevitEditorPlayground.Execution.Contracts;

                       public class Functions
                       {
                        [Script("Some cool script")]
                        public static double DoThing(double value, string[] args)
                        {
                            Console.WriteLine($"Value: {value}");
                            
                            foreach (var arg in args)
                            {
                                Console.WriteLine($"Arg: {arg}");
                            }

                            return value;
                        }
                        
                        [Script("Dummy script")]
                        public static void DummyScript()
                        {
                                                
                        }
                       }
                       """;
        
        var result = InMemoryExecution.RunTest(
            codebase: codebase,
            args: [10, Array.Of("1", "2")],
            assemblyName: "Script1"
        );
        
        var normalizedResult = result.SnapshotNormalize();
        
        InlineSnapshot.Validate(normalizedResult, """
            {
              "error": {
                "code": "MULTIPLE_SCRIPTS_FOUND",
                "description": "Found 2 scripts. Expected 1.",
                "metadata": {
                  "scripts": [
                    {
                      "name": "Some cool script",
                      "type": "Functions",
                      "method": "DoThing"
                    },
                    {
                      "name": "Dummy script",
                      "type": "Functions",
                      "method": "DummyScript"
                    }
                  ]
                },
                "type": "failure",
                "innerErrors": []
              }
            }
            """);
    }

    [Fact]
    public void ShouldBeAbleToExecuteScriptsWithVariousSignatures()
    {
        var testCodeSections = List.Of(
            TestCodeSection.FromFunction(
                body: """
                       public static int AddInts(int a, int b)
                       {
                        return a + b;
                       }
                       """,
                args: [1, 2],
                scriptName: "Add Integers"
                ),
            TestCodeSection.FromFunction(
                body: """
                      public static void EmptyScript()
                      {
                      
                      }
                      """,
                scriptName: "Empty script"
                ),
            TestCodeSection.FromFunction(
                body: """
                       public static int PassVarious(int[] a, string[] b, double someValue)
                       {
                        return 1;
                       }
                       """,
                args: [Array.Of(1, 2), Array.Of("1", "2"), 1.0],
                scriptName: "Pass various"
                )
        );

        var testResults = testCodeSections
            .Select(testCodeSection => InMemoryExecution.RunTest(
                codebase: testCodeSection.Code,
                args: testCodeSection.Args,
                assemblyName: "Script1"
            ));

        InlineSnapshot.Validate(testResults.SnapshotNormalized(), """
            {
              "successCount": 3,
              "failureCount": 0,
              "tests": [
                {
                  "compiledCode": {
                    "bytesCount": 2560,
                    "compilation": {
                      "assemblyName": "Script1",
                      "languageVersion": "cSharp14"
                    }
                  },
                  "assembly": {
                    "fullName": "Script1, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
                  },
                  "executedScript": {
                    "args": [
                      1,
                      2
                    ],
                    "output": 3,
                    "name": "Add Integers"
                  },
                  "events": []
                },
                {
                  "compiledCode": {
                    "bytesCount": 2560,
                    "compilation": {
                      "assemblyName": "Script1",
                      "languageVersion": "cSharp14"
                    }
                  },
                  "assembly": {
                    "fullName": "Script1, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
                  },
                  "executedScript": {
                    "args": null,
                    "output": null,
                    "name": "Empty script"
                  },
                  "events": []
                },
                {
                  "compiledCode": {
                    "bytesCount": 2560,
                    "compilation": {
                      "assemblyName": "Script1",
                      "languageVersion": "cSharp14"
                    }
                  },
                  "assembly": {
                    "fullName": "Script1, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
                  },
                  "executedScript": {
                    "args": [
                      [
                        1,
                        2
                      ],
                      [
                        "1",
                        "2"
                      ],
                      1
                    ],
                    "output": 1,
                    "name": "Pass various"
                  },
                  "events": []
                }
              ]
            }
            """);
    }
}