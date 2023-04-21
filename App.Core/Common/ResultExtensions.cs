using System;

namespace AppCore.Common
{
    public static class ResultExtensions
    {
        public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<T, K> func)
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error);

            return Result.Ok(func(result.Value));
        }

       
        public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<Result<K>> action)
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error);

            return action();
        }

        public static Result OnSuccess(this Result result, Func<Result> func)
        {
            if (result.IsFailure)
                return Result.Fail(result.Error);
            
            return func();
        }

        public static Result<T> OnFailure<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsFailure)
            {
                action(result.Value);
            }

            return result;
        }

        
    }
}
