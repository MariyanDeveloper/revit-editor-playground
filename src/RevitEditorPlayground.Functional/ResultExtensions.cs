namespace Functional;

public static class ResultExtensions
{
    extension<T>(IEnumerable<Result<T>> results)
    {
        public IEnumerable<T> Flatten()
        {
            return results.Where(result => result.IsValid).Select(result => result.Value!);
        }
    }

    extension<T>(Result<T> result)
    {
        public Result<TR> Map<TR>(Func<T, TR> func) =>
            result.Match(valid: t => Results.Ok(func(t)), invalid: Result<TR>.Fail);

        public Result<TR> Then<TR>(Func<T, Result<TR>> func) =>
            result.Match(valid: func, invalid: Result<TR>.Fail);

        public Result<Contextual<TR, T>> ThenWithContext<TR>(Func<T, Result<TR>> func)
        {
            return result.Then(item => func(item).WithContext(item));
        }

        public void Do(Action<T> action)
        {
            if (!result.IsValid)
            {
                return;
            }

            if (result.Value != null)
            {
                action(result.Value);
            }
        }
    }
}
