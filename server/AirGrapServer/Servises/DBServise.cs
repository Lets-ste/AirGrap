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
            Logger.AddErrorLog($"DB check failed: {ex.Message}");
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
        catch (PostgresException ex) when (ex.SqlState == "23502")
        {
            throw new NullInputFieldException();
        }
        catch (PostgresException ex)
        {
            Logger.AddErrorLog($"Registration failed: {ex.Message}", ex.SqlState);
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
        catch (PostgresException ex) when (ex.SqlState == "P0002")
        {
            throw new InvalidCredentialsException();
        }
        catch (PostgresException ex)
        {
            Logger.AddErrorLog($"Auntification failed: {ex.Message}", ex.SqlState);
            throw new RegistrationFailedException();
        }
    }
    public async Task Authorize(string token)
    {
        try
        {
            await using var cmd = _dataBase.CreateCommand("select authorize($1)");
            cmd.Parameters.AddWithValue(token);
            var result = await cmd.ExecuteScalarAsync();
            if (result is int i && i != 1)
                throw new SessionNotFoundException();
        }
        catch (PostgresException ex)
        {
            Logger.AddErrorLog($"Authorize failed: {ex.Message}", ex.SqlState);
            throw new SessionNotFoundException();
        }
    }

}
