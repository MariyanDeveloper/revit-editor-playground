namespace RevitEditorPlayground.Execution.Tests.InMemory.Utils;

public static class InMemoryLibs
{
    public static string Contracts = Path.Combine(
        Directory.GetCurrentDirectory(),
        "InMemory",
        "libs",
        "net48",
        "RevitEditorPlayground.Execution.Contracts.dll"
    );
}