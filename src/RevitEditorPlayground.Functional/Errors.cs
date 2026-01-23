namespace RevitEditorPlayground.Functional;

public static class GenericCodes
{
    public const string GenericFailure = "GENERIC_FAILURE";
}

public static class Errors
{
    extension(Error)
    {
        public static Error Failure(
            string description,
            string? code = null,
            Dictionary<string, object>? metadata = null,
            List<Error>? innerErrors = null
        )
        {
            return new Error(
                code ?? GenericCodes.GenericFailure,
                description,
                metadata,
                ErrorType.Failure,
                innerErrors ?? []
            );
        }
    }
}
