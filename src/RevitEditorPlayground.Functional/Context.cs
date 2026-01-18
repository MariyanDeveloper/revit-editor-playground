namespace Functional;

public record Contextual<T, TContext>(T Value, TContext Context);


public static class ContextualExtensions
{
    public static Result<Contextual<T, TContext>> WithContext<T, TContext>(this Result<T> error, TContext context)
    {
        return error
            .Map(x => new Contextual<T, TContext>(x, context));
    }

    public static Contextual<T, TContext> WithContext<T, TContext>(this T value, TContext context)
    {
        return new Contextual<T, TContext>(value, context);
    }
}