namespace RevitEditorPlayground.Shared;

public static class StringJoining
{
    extension(IEnumerable<string> strings)
    {
        public string JoinBy(string separator)
        {
            return string.Join(separator, strings);
        }
    }
}
