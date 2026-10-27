using System.Text.Json;
using Npgsql;


namespace AirGrap.Datasurse;

public class DataBase
{
    public NpgsqlDataSource dataSource;
    public DataBase()
    {
        ConectionString? ConInf = null;
        if (File.Exists("Datasurse/ConectionSettings.json"))
        {
            var jsonInput = File.ReadAllText("Datasurse/ConectionSettings.json");
            ConInf = JsonSerializer.Deserialize<ConectionString>(jsonInput);
        }
        if (ConInf == null) ConInf = new();
        
        string connectionString = $"Host={ConInf.Host};Port={ConInf.Port};Database={ConInf.Database};Username={ConInf.Username};Password={ConInf.Password}";
        dataSource = NpgsqlDataSource.Create(connectionString);
    }

        
}
