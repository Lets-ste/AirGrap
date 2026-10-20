using Npgsql;
namespace AirGrap.Services;

public class DbService : IDbService
{
    public enum ServerStatus
    {
        OK,
        Error,
        NoConection
    }
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=airgrap;Username=postgres;Password=postgres";

    public async Task<ServerStatus> CheckConnectionAsync()
    {
        ServerStatus status = ServerStatus.NoConection;
        await using var dataSource = NpgsqlDataSource.Create(ConnectionString);

        await using var cmd = dataSource.CreateCommand("SELECT 1");
        try{
            status = (await cmd.ExecuteScalarAsync()) != null ? ServerStatus.OK : ServerStatus.Error;
        }
        finally {}
        return status;
    }
}