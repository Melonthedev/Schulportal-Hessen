using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Schulportal_Hessen.Helpers;
using Schulportal_Hessen.Services;
using Schulportal_Hessen.ViewModels;
using System.Diagnostics;


namespace Schulportal_Hessen.Views;

public sealed partial class MainPage : Page
{

    public MainViewModel ViewModel
    {
        get;
    }

    public AuthService _authService
    {
        get;
    }

    public SpWrapper _SpWrapper
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        _authService = App.GetService<AuthService>();
        _SpWrapper = App.GetService<SpWrapper>();
        Loaded += MainPage_Loaded;
        InitializeComponent();
    }


    private async void MainPage_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadContents();
    }

    public async Task LoadContents()
    {
        if (!await _SpWrapper.AutoLoginAsync())
        {
            WelcomeMessage.Text = "Willkommen";
            return;
        }
        WelcomeMessage.Text = "Willkommen, " + await _SpWrapper.GetSurNameAsync();// TODO SAVE AND LOAD OFFLINE (CACHE)
    }
}
