namespace Functional;

public enum ErrorType
{
    Failure,
}

public record Error(
    string Code,
    string Description,
    Dictionary<string, object>? Metadata,
    ErrorType Type,
    List<Error> InnerErrors
);