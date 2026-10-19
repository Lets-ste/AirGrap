using Npgsql;
public interface IDbService
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=mydb;Username=postgres;Password=secret";

    public async Task<string> CheckConnectionAsync()
    {
        await using var dataSource = NpgsqlDataSource.Create(ConnectionString);

        await using var cmd = dataSource.CreateCommand("SELECT 1");
        string result = (await cmd.ExecuteScalarAsync() ?? "No Result").ToString()!;

        return result;
    }
}