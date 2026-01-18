namespace Functional;

public readonly struct Result<T>
{
    public Error? Error { get; }
    public T? Value { get; }
    public bool IsValid { get; }

    public static Result<T> Fail(Error error) => new(error);
    
    private Result(Error error) =>
        (IsValid, Error, Value) = (false, error, default);

    public Result(T t) => (IsValid, Error, Value) = (true, null, t);

    public static implicit operator Result<T>(Error error) => Fail(error);
    public static implicit operator Result<T>(T t) => new(t ?? throw new ArgumentNullException(nameof(t)));

    public TR Match<TR>(Func<Error, TR> invalid, Func<T, TR> valid)
    {
        if (IsValid)
        {
            var value = Value!;
            return valid(value);
        }
        
        var error = Error!;
        return invalid(error);
    }
    
    public void Match(Action<Error> invalid, Action<T> valid)
    {
        if (IsValid)
        {
            var value = Value!;
            valid(value);
            return;
        }
        
        var error = Error!;
        invalid(error);
    }

    public IEnumerator<T> AsEnumerable()
    {
        if (IsValid)
        {
            yield return Value!;
        }
    }

    public override string ToString() =>
        IsValid ? $"Valid({Value})" : $"Invalid([{string.Join(", ", Error)}])";
    
    
}