using AirGrap.Services;
using AirGrap.Datasurse;
namespace AirGrap;
class Program
{
    static void Main(string[] args)
    {
        DataBase dataBase = new();

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton(dataBase.dataSource);
        builder.Services.AddSingleton<IDbService, DbService>();
        
        builder.Services.AddControllers();
        var app = builder.Build();
        app.MapControllers();
        app.Run();
    }
    
}