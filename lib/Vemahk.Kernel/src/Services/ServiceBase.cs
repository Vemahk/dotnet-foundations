using Microsoft.Extensions.Logging;

namespace Vemahk.Kernel.Services;

public abstract class ServiceBase<TService>
    where TService : ServiceBase<TService>
{
    protected ILogger<TService> Logger { get; }

    protected ServiceBase(ILogger<TService> logger)
    {
        Logger = logger;
    }

    protected static Result Pass() => Result.Pass();
    protected static Result<T> Pass<T>(T data) => Result.Pass(data);
    protected static Result<Optional<T>> Some<T>(T data) where T : notnull => Result.Some(data);
    protected static Result<Optional<T>> None<T>() where T : notnull => Result.None<T>();
    protected static FailedResult Fail(string reason) => Result.Fail(reason);
    protected static FailedResult Fail(Result other) => Result.Fail(other);

    protected FailedResult UnexpectedError(Exception e, string message = "An unexpected error occurred.") => ProcessException(LogLevel.Error, e, message);
    protected FailedResult CriticalError(Exception e, string message = "A critical error occurred.") => ProcessException(LogLevel.Critical, e, message);

    protected FailedResult ProcessException(LogLevel logLevel, Exception e, string? message)
    {
        if(string.IsNullOrWhiteSpace(message))
            Logger.Log(logLevel, e, string.Empty);
        else
            Logger.Log(logLevel, e, "{message}", message);
            
        return Fail(message ?? e.Message);
    }
}