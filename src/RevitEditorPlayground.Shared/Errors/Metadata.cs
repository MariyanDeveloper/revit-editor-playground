using RevitEditorPlayground.Functional;

namespace RevitEditorPlayground.Shared.Errors;

public static class Metadata
{
    extension(Error)
    {
        public static Dictionary<string, object> ExceptionMetadata(Exception exception)
        {
            return new Dictionary<string, object>()
            {
                ["type"] = exception.GetType().Name,
                ["message"] = exception.Message,
                ["stackTrace"] = exception.StackTrace,
                ["innerException"] = exception.InnerException,
            };
        }
    }
}