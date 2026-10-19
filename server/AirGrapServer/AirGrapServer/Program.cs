using AirGrap.Services;
namespace AirGrap;
class Program
{
    static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<IDbService, DbService>();
        builder.Services.AddControllers();
        var app = builder.Build();

        /*app.Use(async (context, next) =>
        {
            var start = DateTime.Now;
            Console.WriteLine($"[{start:HH:mm:ss}] → {context.Request.Method} {context.Request.Path}");
            await next();
            var ms = (DateTime.Now - start).TotalMilliseconds;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ← {context.Response.StatusCode} ({ms:F0} мс)");
        });

        app.Use(async (context, next) =>
        {
            if (context.Request.Query.ContainsKey("secret"))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Доступ запрещён: секретный параметр");
                return;
            }
            await next();
        });*/
        app.MapControllers();
        app.Run();
    }
    
}