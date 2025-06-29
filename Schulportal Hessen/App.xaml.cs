﻿using System.Diagnostics;
using System.Net.Http;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

using Schulportal_Hessen.Activation;
using Schulportal_Hessen.Contracts.Services;
using Schulportal_Hessen.Core.Contracts.Services;
using Schulportal_Hessen.Core.Services;
using Schulportal_Hessen.Helpers;
using Schulportal_Hessen.Models;
using Schulportal_Hessen.Notifications;
using Schulportal_Hessen.Services;
using Schulportal_Hessen.ViewModels;
using Schulportal_Hessen.Views;

namespace Schulportal_Hessen;

public partial class App : Application {
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host {
        get;
    }

    public static T GetService<T>()
        where T : class {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service) {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App() {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host
        .CreateDefaultBuilder()
        .UseContentRoot(AppContext.BaseDirectory)
        .ConfigureServices((context, services) => {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

            // Services
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<IWebViewService, WebViewService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<ISampleDataService, SampleDataService>();
            services.AddSingleton<IFileService, FileService>();

            services.AddSingleton<NetworkService>();
            services.AddSingleton<ErrorService>();
            services.AddSingleton<AuthService>();
            services.AddSingleton<SpWrapper>();
            services.AddSingleton<TimeTableService>();
            services.AddSingleton<SettingsService>();

            // Views and ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<LoginPage>();
            services.AddSingleton<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<ChatsViewModel>();
            services.AddTransient<ChatsPage>();
            services.AddTransient<InhaltsrasterDetailViewModel>();
            services.AddTransient<InhaltsrasterDetailPage>();
            services.AddTransient<InhaltsrasterViewModel>();
            services.AddTransient<InhaltsrasterPage>();
            services.AddTransient<WebansichtViewModel>();
            services.AddTransient<WebansichtPage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<TimetableViewModel>();
            services.AddTransient<TimetablePage>();
            services.AddTransient<CoursesViewModel>();
            services.AddTransient<CoursesPage>();
            services.AddTransient<SubstitutionsViewModel>();
            services.AddTransient<SubstitutionsPage>();
            services.AddTransient<FirstSubstitutionsViewModel>();
            services.AddTransient<FirstSubstitutionsPage>();
            services.AddTransient<SecondSubstitutionsViewModel>();
            services.AddTransient<SecondSubstitutionsPage>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        App.GetService<IAppNotificationService>().Initialize();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e) {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
        //Debug.WriteLine(e.Exception.ToString());
        /*if (e.Exception is HttpRequestException)
        {
            App.GetService<NetworkService>().ShowNetworkError();
            e.Handled = true;
        }*/
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args) {
        base.OnLaunched(args);

        App.GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
