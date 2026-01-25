namespace RevitEditorPlayground.Functional;

public static class OptionExtensions
{
    extension<T>(Option<T> option)
    {
        public T OrElse(T defaultValue)
        {
            return option.Match(none: () => defaultValue, some: x => x);
        }
    }
}