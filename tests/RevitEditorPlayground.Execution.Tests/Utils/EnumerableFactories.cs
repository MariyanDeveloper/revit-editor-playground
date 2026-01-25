namespace RevitEditorPlayground.Execution.Tests.Utils;

public static class List;

public static class ListFactories
{
    extension(List)
    {
        public static List<T> Of<T>(params T[] items)
        {
            return [..items];
        }
    }
}
public static class ArrayFactories
{
    extension(Array)
    {
        public static T[] Of<T>(params T[] items)
        {
            return items;
        }
    }
}