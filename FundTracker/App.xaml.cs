using FundTracker.Activation;
using FundTracker.Contracts.Services;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Services;
using FundTracker.Helpers;
using FundTracker.Models;
using FundTracker.Notifications;
using FundTracker.Services;
using FundTracker.ViewModels;
using FundTracker.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;

namespace FundTracker;

public partial class App : Application
{
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }
       

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            services.AddLogging(builder => builder.AddDebug());
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

            // Services
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IRepositoryService, RepositoryService>();
            services.AddSingleton<IMorningStarService, MorningStarService>();
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<PortfoliosViewModel>();
            services.AddTransient<PortfoliosPage>();
            services.AddTransient<FundsViewModel>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        LoadRepository();
        App.GetService<IAppNotificationService>().Initialize();

        UnhandledException += App_UnhandledException;
    }

    private static string GetApplicationStoragePath()
    {
        var localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var applicationDataFolder = Path.Combine(localApplicationData, "FundTracker/ApplicationData");
        return Path.Combine(applicationDataFolder, "FundTracker.ftf");
    }

    public async void LoadRepository()
    {
        var repositoryService = App.GetService<IRepositoryService>();
        var applicationStoragePath = GetApplicationStoragePath();

        if (!File.Exists(applicationStoragePath))
        {
            return;
        }

        var storageStream = new FileStream(GetApplicationStoragePath(), FileMode.Open, FileAccess.Read);

        await Task.Run(() => repositoryService.Load(storageStream));

        storageStream.Close();
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        var logger = Host.Services.GetService<ILogger>();
        logger?.LogError($"Exception d'application non gérée : {e.Exception},{e.Message}");
    }

    public static async void SaveRepository()
    {
        var repositoryService = App.GetService<IRepositoryService>();
        var applicationStoragePath = GetApplicationStoragePath();
        var storageStream = new FileStream(applicationStoragePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        await Task.Run(() =>repositoryService.Save(storageStream));

        storageStream.Close();
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        App.GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
