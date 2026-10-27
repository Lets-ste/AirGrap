using Npgsql;
using AirGrap.Datasurse;
using AirGrap.Exceptions;
namespace AirGrap.Services;

public class DbService (NpgsqlDataSource _dataBase) : IDbService
{

    public async Task<ServerStatus> CheckConnectionAsync()
    {
        try
        {
            await using var cmd = _dataBase.CreateCommand("SELECT 1");
            var raw = await cmd.ExecuteScalarAsync();
            return raw is int i && i == 1 ? ServerStatus.OK : ServerStatus.Error;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DB check failed: {ex.Message}");
            return ServerStatus.NoConection;
        }
    }
    public async Task Registration(
        string login,string Password,string nicname)
    {
        
        try
        {
            await using var cmd = _dataBase.CreateCommand("call registration($1,$2,$3);");
            cmd.Parameters.AddWithValue(login);
            cmd.Parameters.AddWithValue(Password);
            cmd.Parameters.AddWithValue(nicname);
            await cmd.ExecuteNonQueryAsync();
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            throw new UserAlreadyExistsException(login);
        }
        catch (PostgresException ex)
        {
            Console.WriteLine($"Registration failed: {ex.Message}");
            throw new RegistrationFailedException();
        }
    }
    public async Task<string?> Auntification(string login,string Password)
    {
        try
        {
            await using var cmd = _dataBase.CreateCommand("select auntification($1,$2)");
            cmd.Parameters.AddWithValue(login);
            cmd.Parameters.AddWithValue(Password);
            string? token = (await cmd.ExecuteScalarAsync() ?? String.Empty).ToString();
            return token;
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        { 
            throw new UserAlreadyExistsException(login);
        }
        catch (PostgresException ex)
        {
            Console.WriteLine($"Registration failed: {ex.Message}");
            throw new RegistrationFailedException();
        }
    }

}