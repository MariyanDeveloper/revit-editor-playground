namespace RevitEditorPlayground.Execution.Tests.InProcess.Utils;

public static class InProcessLibs
{
    public static string NewtonsoftJson = Path.Combine(
        Directory.GetCurrentDirectory(),
        "InProcess",
        "libs",
        "net45",
        "Newtonsoft.Json.dll"
    );
}