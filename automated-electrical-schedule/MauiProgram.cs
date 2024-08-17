using automated_electrical_schedule.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace automated_electrical_schedule;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlite($"Data Source={DatabaseContext.DbPath}"));


#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        dbContext.Database.EnsureCreated();

        return app;
    }
}