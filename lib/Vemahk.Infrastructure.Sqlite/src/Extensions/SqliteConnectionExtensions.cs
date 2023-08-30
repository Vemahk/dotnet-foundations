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
}