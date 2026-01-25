namespace RevitEditorPlayground.Shared;

public static class Assertions
{
    extension<T>(T? value)
    {
        public T NotNull()
        {
            return value ?? throw new NullReferenceException("Expected value to be not null");
        }
    }
}