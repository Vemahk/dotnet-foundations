namespace Vemahk.Kernel.Services
{
    public class Response
    {
        public bool Success { get; }
        public string Message { get; }

        internal Response(bool success, string message = null)
        {
            Success = success;
            Message = message ?? string.Empty;
        }

        public static Response Pass() => new Response(true);
        public static Response<T> Pass<T>(T data) => new Response<T>(data, true);
        public static FailureResponse Fail(string reason) => new FailureResponse(reason);
        public static FailureResponse Fail(Response other) => new FailureResponse(other.Message);

        public static implicit operator Response(FailureResponse failure) => new Response(false, failure.Message);
    }

    public sealed class Response<T> : Response
    {
        public T Data { get; }

        internal Response(T data, bool success, string message = null)
            : base(success, message)
        {
            Data = data;
        }

        public static implicit operator Response<T>(FailureResponse failure) => new Response<T>(default, false, failure.Message);
    }

    public sealed class FailureResponse
    {
        public string Message { get; }

        internal FailureResponse(string message)
        {
            Message = message;
        }
    }
}