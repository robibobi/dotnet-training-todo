using System;

namespace TodoApplication.Util
{
    public interface Result<T>
    {
        T Value { get; }

        string Message { get; }
    
        bool WasSuccessful { get; }
    }

    public class Success<T> : Result<T>
    {
        public T Value { get; }
        public string Message => throw new ArgumentException("Success does not have an error.");
        public bool WasSuccessful => true;

        public Success(T value)
        {
            Value = value;
        }
    }

    public class Error<T> : Result<T>
    {
        public T Value => throw new ArgumentException("Error does not have a value.");
        public bool WasSuccessful => false;
        public string Message { get; }

        public Error(string message)
        {
            Message = message;
        }
    }

    public static class Result
    {
        private static readonly Result<Unit> UnitSuccess = new Success<Unit>(Unit.Default);

        public static Result<T> CreateSuccess<T>(T value)
        {
            return new Success<T>(value);
        }

        public static Result<Unit> CreateSuccess()
        {
            return UnitSuccess;
        }
    }



}
