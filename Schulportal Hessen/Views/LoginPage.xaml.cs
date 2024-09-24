using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Schulportal_Hessen.Helpers;
using Schulportal_Hessen.Services;
using Schulportal_Hessen.ViewModels;
using System.Diagnostics;


namespace Schulportal_Hessen.Views;

public sealed partial class LoginPage : Page
{
    public LoginViewModel ViewModel
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

    public LoginPage()
    {
        ViewModel = App.GetService<LoginViewModel>();
        _AuthService = App.GetService<AuthService>();
        _SpWrapper = App.GetService<SpWrapper>();
        InitializeComponent();

        Debug.WriteLine("TZEST");
    }

    private void RevealModeCheckbox_Changed(object sender, RoutedEventArgs e)
    {
        if (revealModeCheckBox.IsChecked == true)
        {
            passworBoxWithRevealmode.PasswordRevealMode = PasswordRevealMode.Visible;
        }
        else
        {
            passworBoxWithRevealmode.PasswordRevealMode = PasswordRevealMode.Hidden;
        }
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        LoginWorking.IsActive = true;

        _AuthService.SaveLoginUrl("https://login.schulportal.hessen.de/?i=8606");
        _AuthService.SaveSchoolId("8606");
        if (usernameInput.Text.Length > 1 && passworBoxWithRevealmode.Password.Length > 1) _AuthService.SaveCredentials(usernameInput.Text, passworBoxWithRevealmode.Password);
        await _AuthService.HandleAuthorizationRequestAsync();

        var name = await _SpWrapper.GetFullName();
        Debug.WriteLine(name);
        Username.Text += name;

        LoginWorking.IsActive = false;
    }

    private async void FetchInfoButton_Click(object sender, RoutedEventArgs e)
    {
        var text = "Geburtstag: " + await _SpWrapper.GetDateOfBirth() + " - Klasse: " + await _SpWrapper.GetSchoolClass();
        InfoBox.Text = text;
    }

    private void ClearCredentials_Click(object sender, RoutedEventArgs e)
    {
        _AuthService.DeleteCredentials();

    }
}
