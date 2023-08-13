namespace Vemahk.Kernel.Services;

public readonly struct Response
{
    public bool Success { get; }
    public string Message { get; }

    internal Response(bool success, string? message = null)
    {
        Success = success;
        Message = message ?? string.Empty;
    }

    public static Response Pass() => new(true);
    public static Response<T> Pass<T>(T data) => new(true, data);
    public static FailureResponse Fail(string reason) => new FailureResponse(reason);
    public static FailureResponse Fail(Response other) => new FailureResponse(other.Message);

    public static implicit operator Response(FailureResponse failure) => new(false, failure.Message);
}

public struct Response<T>
{
    public bool Success { get; }
    public T Data { get; }
    public string Message { get; }

    internal Response(bool success, T data, string? message = null)
    {
        Success = success;

        Data = data;
        Message = message ?? string.Empty;
    }

    public static implicit operator Response<T>(FailureResponse failure) => new(false, default!, failure.Message);
}

public readonly struct FailureResponse
{
    public string Message { get; }

    internal FailureResponse(string message)
    {
        Message = message;
    }
}