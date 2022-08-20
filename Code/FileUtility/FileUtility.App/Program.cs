
using Microsoft.Extensions.Configuration;
using Serilog;
using FileUtility.App;
using Microsoft.Extensions.DependencyInjection;
using FileUtility.App.Model;
using FileUtility.Data;

public class Program
{
    /// <summary>
    /// Confoguration for the Application
    /// </summary>
    public static IConfiguration Configuration { get; private set; }

    /// <summary>
    /// Entry point of project
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static async Task Main(string[] args)
    {
        try
        {
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                        .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                                        .Build();
            // configure serilog
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration)
                                                  .Enrich.FromLogContext()
                                                  .Enrich.WithMachineName()
                                                  .CreateLogger();
            Log.Information("Starting up...");

            // Create service collection and configure our services
            var services = ConfigureServices();
            // Generate a provider
            var serviceProvider = services.BuildServiceProvider();

            // Kick off the actual application
            await serviceProvider.GetRequiredService<FileUpload>().Run();

        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IServiceCollection ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        // register the services

        // Configuration should be singleton as the entire application should use one
        services.AddSingleton(Configuration);
        // for strongly typed options to be injected as IOption<T> in constructors
        services.AddOptions();
        // Configure EmailSettings so IOption<ApplicationConfig> can be injected 
        services.Configure<ApplicationConfig>(Configuration.GetSection("ApplicationConfig"));

        // Register the actual application entry point
          services.AddTransient<FileUpload>();
        services.AddTransient<IFileUploadService, FileUploadService>();
      

        return services;
    }
}