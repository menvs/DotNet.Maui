using CommunityToolkit.Maui;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using QlHui.App.Data;
using System.Reflection;

namespace QlHui.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddRazorPages();
        builder.Services.AddMvcCore().AddRazorRuntimeCompilation();
        builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(opts =>
        {
            opts.FileProviders.Add(new PhysicalFileProvider(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Views"))); // This will be the root path
        });
        builder.Services.AddRazorTemplating();
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        builder.Configuration.AddConfiguration(config);


#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif
        builder.Services.InjectService();

        return builder.Build();
    }
}
