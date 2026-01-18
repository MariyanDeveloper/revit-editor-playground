using Functional;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace RevitEditorPlayground.Compilation.Utils;

public static class Emitting
{
    extension(CSharpCompilation compilation)
    {
        public Result<IReadOnlyList<byte>> EmitBytes(Option<EmitOptions> emitOptions)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                var emitResult = compilation.Emit(peStream: memoryStream, options: emitOptions.InternalValue());

                if (!emitResult.Success)
                {
                    return Error.Failure(
                        description: "Failed compilation",
                        metadata: new Dictionary<string, object>
                        {
                            ["diagnostics"] = emitResult.Diagnostics
                        }
                    );
                }

                memoryStream.Seek(0, SeekOrigin.Begin);

                return memoryStream.ToArray();
            }
            catch (Exception e)
            {
                return Error.Failure(description: $"Failed to emit compilation: {e.Message}. {e.StackTrace}");
            }
        }
    }
}