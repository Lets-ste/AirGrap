using Npgsql;
using AirGrap.Datasurse;
public interface IDbService
{
    public Task<ServerStatus> CheckConnectionAsync();
    public Task Registration(string login,string Password,string nicname);
}