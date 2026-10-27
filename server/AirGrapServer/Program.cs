using Microsoft.AspNetCore.OpenApi;
using AirGrap.Services;
using AirGrap.Datasurse;
using Scalar.AspNetCore;
namespace AirGrap;
class Program
{
    static void Main(string[] args)
    {
        DataBase dataBase = new();

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton(dataBase.dataSource);
        builder.Services.AddSingleton<IDbService, DbService>();
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        var app = builder.Build();
        app.MapControllers();
        app.MapOpenApi();
        app.MapScalarApiReference();
        app.Run();
    }
    
}