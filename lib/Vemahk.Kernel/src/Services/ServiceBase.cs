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

    protected static Response Pass() => Response.Pass();
    protected static Response<T> Pass<T>(T data) => Response.Pass(data);
    protected static FailureResponse Fail(string reason) => Response.Fail(reason);
    protected static FailureResponse Fail(Response other) => Response.Fail(other);

    protected FailureResponse UnexpectedError(Exception e, string message = "An unexpected error occurred.") => ProcessException(LogLevel.Error, e, message);
    protected FailureResponse CriticalError(Exception e, string message = "A critical error occurred.") => ProcessException(LogLevel.Critical, e, message);

    protected FailureResponse ProcessException(LogLevel logLevel, Exception e, string? message)
    {
        if(string.IsNullOrWhiteSpace(message))
            Logger.Log(logLevel, e, string.Empty);
        else
            Logger.Log(logLevel, e, "{message}", message);
            
        return Fail(message ?? e.Message);
    }
}