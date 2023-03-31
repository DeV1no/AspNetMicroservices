using Npgsql;

namespace Discount.Api.Extensions;

public static class HostExtensions
{
    public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
    {
        var retryForAvailability = retry!.Value;
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Migrating postresql database");
                using var connection =
                   new NpgsqlConnection(configuration
                       .GetValue<string>("DatabaseSettings:ConnectionString"));
                connection.Open();
                using var command = new NpgsqlCommand { Connection = connection };
                command.CommandText = "Drop Table If Exist";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE Coupon (Id SERIAL PRIMARY KEY,
                                                               ProductName VARCHAR(24) NOTT NULL,
                                                               Description TEXT,
                                                               Amount INT)";
                command.ExecuteNonQuery();

                logger.LogInformation("Migrating Done");
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while migrating the postresql database");

            }
            return host;
        }
    }
}