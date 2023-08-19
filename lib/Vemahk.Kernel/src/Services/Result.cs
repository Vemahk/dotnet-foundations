using System.Text.Json.Serialization;

namespace Vemahk.Kernel.Services;

public readonly struct Result
{
    public bool Success { get; }
    public string Message { get; }

    [JsonConstructor]
    public Result(bool success, string? message = null)
    {
        Success = success;
        Message = message ?? string.Empty;
    }

    public static Result Pass() => new(true);
    public static Result<T> Pass<T>(T data) => new(true, data);
    public static Result<Optional<T>> Some<T>(T data) where T : notnull => new(true, data);
    public static Result<Optional<T>> None<T>() where T : notnull => new(true, Optional<T>.None);
    public static FailedResult Fail(string reason) => new FailedResult(reason);
    public static FailedResult Fail(Result other) => Fail(other.Message);
    public static FailedResult Fail<T>(Result<T> other) => Fail(other.Message);

    public static implicit operator Result(FailedResult failure) => new(false, failure.Message);
}
public struct Result<T>
{
    public bool Success { get; }
    public T Data { get; }
    public string Message { get; }

    [JsonConstructor]
    public Result(bool success, T data, string? message = null)
    {
        Success = success;

        Data = data;
        Message = message ?? string.Empty;
    }

    public static implicit operator Result<T>(FailedResult failure) => new(false, default!, failure.Message);
}

public readonly struct FailedResult
{
    public string Message { get; }

    internal FailedResult(string message)
    {
        Message = message;
    }
}