using Npgsql;
namespace AirGrap.Services;

public class DbService : IDbService
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=airgrap;Username=postgres;Password=postgres";

    public async Task<string> CheckConnectionAsync()
    {
        await using var dataSource = NpgsqlDataSource.Create(ConnectionString);

        await using var cmd = dataSource.CreateCommand("SELECT 1");
        string result = (await cmd.ExecuteScalarAsync() != null ? "Data base is conected":"Dsconect to Data base").ToString()!;

        return result;
    }
}