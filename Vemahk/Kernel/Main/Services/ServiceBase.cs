using System;
using Microsoft.Extensions.Logging;

namespace Vemahk.Kernel.Services
{
    public abstract class ServiceBase<TService>
        where TService : ServiceBase<TService>
    {
        protected readonly ILogger<TService> Logger;

        protected ServiceBase(ILogger<TService> logger)
        {
            Logger = logger;
        }

        protected static Response Pass() => Response.Pass();
        protected static Response<T> Pass<T>(T data) => Response.Pass(data);
        protected static FailureResponse Fail(string reason) => Response.Fail(reason);
        protected static FailureResponse Fail(Response other) => Response.Fail(other);

        protected FailureResponse UnexpectedError(Exception e, string message = null) => ProcessException(LogLevel.Error, e, message);
        protected FailureResponse CriticalError(Exception e, string message = null) => ProcessException(LogLevel.Critical, e, message);

        protected FailureResponse ProcessException(LogLevel logLevel, Exception e, string message = null)
        {
            message = message ?? e.Message;
            Logger.Log(logLevel, "{message} {exception}", message, e);
            return Fail(message ?? e.Message);
        }
    }
}