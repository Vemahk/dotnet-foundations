using System.Data;

using Vemahk.Kernel.Services;

namespace Vemahk.Infrastructure.Extensions;

public static class DbReaderExtensions
{
    public static Optional<T> GetOptional<T>(this IDataReader reader, string columnName, Func<int, T> getter)
        where T : notnull
    {
        var ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
            return Optional<T>.None;
        return getter(ordinal);
    }

    public static T GetValue<T>(this IDataReader reader, string columnName, Func<int, T> getter)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return getter(ordinal);
    }

    public static Guid GetGuid(this IDataReader reader, string columnName) => reader.GetValue(columnName, reader.GetGuid);
    public static Optional<Guid> GetOptionalGuid(this IDataReader reader, string columnName) => reader.GetOptional(columnName, reader.GetGuid);

    public static long GetLong(this IDataReader reader, string columnName) => reader.GetValue(columnName, reader.GetInt64);
    public static Optional<long> GetOptionalLong(this IDataReader reader, string columnName) => reader.GetOptional(columnName, reader.GetInt64);

    public static ulong GetULong(this IDataReader reader, string columnName) => (ulong)reader.GetLong(columnName);
    public static Optional<ulong> GetOptionalULong(this IDataReader reader, string columnName) => reader.GetOptionalLong(columnName).Map(x => (ulong)x);

    public static string GetString(this IDataReader reader, string columnName) => reader.GetValue(columnName, reader.GetString);
    public static Optional<string> GetOptionalString(this IDataReader reader, string columnName) => reader.GetOptional(columnName, reader.GetString);
}
