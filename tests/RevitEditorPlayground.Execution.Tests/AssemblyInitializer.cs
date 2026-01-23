using System.Text.Json;
using System.Text.Json.Serialization;
using Meziantou.Framework.InlineSnapshotTesting;
using Meziantou.Framework.InlineSnapshotTesting.Serialization;

namespace RevitEditorPlayground.Execution.Tests;

static class AssemblyInitializer
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void Initialize()
    {
        InlineSnapshotSettings.Default = InlineSnapshotSettings.Default with
        {
            
            SnapshotSerializer = new JsonSnapshotSerializer(new JsonSerializerOptions(
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                }
            ))
        };
    }
}