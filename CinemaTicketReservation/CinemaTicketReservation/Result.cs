namespace CinemaTicketReservation
{
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public string Error { get; protected set; } = "";

        public static Result Success() => new Result { IsSuccess = true };
        public static Result Failure(string error) => new Result { IsSuccess = false, Error = error };
    }

    public class Result<T> : Result
    {
        public T? Value { get; private set; }

        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value};
        public static new Result<T> Failure(string error) => new Result<T> { IsSuccess = false, Error = error };

    }
}
