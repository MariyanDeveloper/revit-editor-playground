namespace RevitEditorPlayground.Execution.Tests.InMemory.Utils;

public record TestCodeSection(
    string Code,
    object?[]? Args
);

public static class TestCodeSections
{
    extension(TestCodeSection)
    {
        public static TestCodeSection FromFunction(
            string body,
            object?[]? args = null,
            string? scriptName = null, string? className = null)
        {
            className ??= "Functions";
            scriptName ??= "Script";
            
            var template = $$"""
                             using System;
                             using RevitEditorPlayground.Execution.Contracts;

                             public class {{className}}
                             {
                              [Script("{{scriptName}}")]
                              {{body}}
                             }
                             """;
            
            return new TestCodeSection(Code: template, Args: args);
            
        }
    }
}