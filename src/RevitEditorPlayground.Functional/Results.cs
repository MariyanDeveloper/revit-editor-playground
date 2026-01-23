namespace RevitEditorPlayground.Functional;

public static class Results
{
    public static Result<T> Ok<T>(T value) =>
        new(value ?? throw new ArgumentNullException(nameof(value)));

    public static Result<T> Fail<T>(Error error) => Result<T>.Fail(error);

    public static Result<T> TryCatch<T>(Func<T> func, Func<Exception, Error> failure)
    {
        try
        {
            var result = func();
            return Ok(result);
        }
        catch (Exception e)
        {
            var error = failure(e);

            return Fail<T>(error);
        }
    }

    public static Result<T> TryCatch<T>(Func<Result<T>> func, Func<Exception, Error> failure)
    {
        try
        {
            var result = func();
            return result;
        }
        catch (Exception e)
        {
            var error = failure(e);

            return Fail<T>(error);
        }
    }
}
