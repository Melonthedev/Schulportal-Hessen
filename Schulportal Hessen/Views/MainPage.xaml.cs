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

    public AuthService _AuthService
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
        _AuthService = App.GetService<AuthService>();
        _SpWrapper = App.GetService<SpWrapper>();
        InitializeComponent();
        LoadContents();

    }

    public async Task LoadContents()
    {
        await _SpWrapper.AutoLoginAsync();
        Debug.WriteLine(_SpWrapper.isLoggedIn + "MAIINOPAE");
        WelcomeMessage.Text = "Willkommen, " + await _SpWrapper.GetFullNameAsync();
    }
}
