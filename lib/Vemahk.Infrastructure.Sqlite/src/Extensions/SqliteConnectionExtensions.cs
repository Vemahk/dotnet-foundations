using System.Data;

using Microsoft.Data.Sqlite;

namespace Vemahk.Infrastructure.Sqlite.Extensions;

public static class SqliteConnectionExtensions
{
    public static SqliteCommand GetTextCommand(this SqliteConnection connection, string text)
    {
        var command = connection.CreateCommand();
        command.CommandText = text;
        command.CommandType = CommandType.Text;

        return command;
    }

    public static async Task<int> GetDatabaseVersion(this SqliteConnection connection, CancellationToken token)
    {
#if NET7_0_OR_GREATER
        await /*what a stupid reason to have to multitarget*/
#endif
        using var cmd = connection.GetTextCommand("PRAGMA user_version;");
        var result = await cmd.ExecuteScalarAsync(token);
        return Convert.ToInt32(result);
    }

    public static async Task SetDatabaseVersion(this SqliteConnection connection, int newVersion, CancellationToken token)
    {
#if NET7_0_OR_GREATER
        await 
#endif
        using var cmd = connection.GetTextCommand($"PRAGMA user_version = {newVersion};");
        await cmd.ExecuteNonQueryAsync(token);
    }
}