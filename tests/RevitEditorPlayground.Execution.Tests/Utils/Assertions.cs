using RevitEditorPlayground.Functional;
using RevitEditorPlayground.Shared;
using Shouldly;

namespace RevitEditorPlayground.Execution.Tests.Utils;

public static class Assertions
{
    extension<T>(Result<T> result)
    {
        public T ShouldHaveValue()
        {
            result.IsValid.ShouldBeTrue();

            return result.Value.NotNull();
        }
    }
}