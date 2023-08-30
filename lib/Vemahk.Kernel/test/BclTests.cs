namespace Vemahk.Kernel.Test;

[TestFixture]
public class BclTests
{
    [Test]
    public void ThenDefaultNullableStructIsNull()
    {
        int? test = default;
        Assert.That(test is null);
    }

    [Test]
    public void ThenDefaultNullableFromGenericIsNull()
    {
        static T Result<T>() => default!;
        Assert.That(Result<int?>() is null, "int?");
    }
}