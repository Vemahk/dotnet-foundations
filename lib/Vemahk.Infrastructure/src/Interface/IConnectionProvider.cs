namespace Vemahk.Infrastructure.Interface
{
    public interface IConnectionProvider<T>
    {
        T GetConnection(string connectionName);
    }
}
