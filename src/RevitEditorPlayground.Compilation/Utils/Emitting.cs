using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using RevitEditorPlayground.Functional;

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

                var emitResult = compilation.Emit(
                    peStream: memoryStream,
                    options: emitOptions.InternalValue()
                );

                if (!emitResult.Success)
                {
                    return Error.FailedCompilation(emitResult);
                }

                memoryStream.Seek(0, SeekOrigin.Begin);

                return memoryStream.ToArray();
            }
            catch (Exception e)
            {
                return Error.UnexpectedEmiting(e);
            }
        }
    }
}
