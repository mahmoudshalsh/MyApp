using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure;

public class MyDbLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
        => new DbLogger(categoryName);

    public void Dispose() { }
}

public class DbLogger(string categoryName) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {

        if (!IsEnabled(logLevel))
            return;

        var message = formatter(state, exception);

        // Log to console for demonstration purposes
        Console.WriteLine($"[{DateTime.UtcNow}] {logLevel} - {eventId.Id} - {message} {exception}");


        // Log this message to the database using Ado.net
        //using var connection = new System.Data.SqlClient.SqlConnection("");
        //connection.Open();
        //using var command = connection.CreateCommand();
        //command.CommandText = "INSERT INTO Logs (LogLevel, Message, Exception, EventId, CreatedAt) VALUES (@LogLevel, @Message, @Exception, @EventId, @CreatedAt)";
        //command.Parameters.AddWithValue("@LogLevel", logLevel.ToString());
        //command.Parameters.AddWithValue("@Message", message);
        //command.Parameters.AddWithValue("@Exception", exception?.ToString() ?? (object)DBNull.Value);
        //command.Parameters.AddWithValue("@EventId", eventId.Id);
        //command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
        //command.ExecuteNonQuery();
    }
}
