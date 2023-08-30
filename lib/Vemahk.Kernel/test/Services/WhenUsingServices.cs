using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Vemahk.Kernel.Services;

namespace Infrastructure.Test.Services;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class WhenUsingServices
{
    [Test]
    public async Task ThenServicesAreUsable()
    {
        var service = new TestService(new NullLogger<TestService>());

        const int thingsToDo = 5;

        var response = await service.DoThing(thingsToDo, default);
        Assert.That(response.Success, "Service call failed.");
        Assert.That(service.ThingsDone, Is.EqualTo(thingsToDo), "Did not do the right work at the right time");
    }

    private class TestService : ServiceBase<TestService>
    {
        private volatile int _thingsDone = 0;
        public int ThingsDone => _thingsDone;

        public TestService(ILogger<TestService> logger) : base(logger)
        {
        }

        public async Task<Result> DoThing(int i, CancellationToken token)
        {
            try
            {
                Interlocked.Add(ref _thingsDone, i);
                return Pass();
            }
            catch (Exception e)
            {
                return UnexpectedError(e);
            }
        }

    }
}