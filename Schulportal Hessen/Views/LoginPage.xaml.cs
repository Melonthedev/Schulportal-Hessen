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

    public List<(string, int)> schools;
    public int selectedSchool;

    public LoginPage()
    {
        ViewModel = App.GetService<LoginViewModel>();
        _AuthService = App.GetService<AuthService>();
        _SpWrapper = App.GetService<SpWrapper>();
        InitializeComponent();

        Debug.WriteLine("TZEST");
        LoadSchoolIdsAsync();
    }

    public async Task LoadSchoolIdsAsync()
    {
       schools = await _SpWrapper.GetSchoolIdsAsync();
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

        Debug.WriteLine(selectedSchool);
        if (selectedSchool != 0)
        {
            _AuthService.SaveSchoolId(selectedSchool.ToString());
            _AuthService.SaveLoginUrl("https://login.schulportal.hessen.de/?i=" + selectedSchool.ToString());
        }
        if (usernameInput.Text.Length > 1 && passworBoxWithRevealmode.Password.Length > 1) _AuthService.SaveCredentials(usernameInput.Text, passworBoxWithRevealmode.Password);
        await _AuthService.HandleAuthorizationRequestAsync();

        var name = await _SpWrapper.GetFullNameAsync();
        Debug.WriteLine(name);
        Username.Text += name;

        LoginWorking.IsActive = false;
        //TODO: Weiterleitung auf home, welcome marlon, login kachel hide, account kachel show, auto startup login, etc
        this.Frame.Navigate(typeof(MainPage));
    }

    private async void FetchInfoButton_Click(object sender, RoutedEventArgs e)
    {
        var text = "Geburtstag: " + await _SpWrapper.GetDateOfBirthAsync() + " - Klasse: " + await _SpWrapper.GetSchoolClassAsync();
        InfoBox.Text = text;
    }

    private void ClearCredentials_Click(object sender, RoutedEventArgs e)
    {
        _AuthService.DeleteCredentials();

    }

    private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        
        // Since selecting an item will also change the text,
        // only listen to changes caused by user entering text.
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            if (schools.Count <= 0)
            {
                await LoadSchoolIdsAsync();
            }
            sender.ItemsSource = schools;
            //LoginButton.IsEnabled = false;
            var suitableItems = new List<(string, int)>();
            var splitText = sender.Text.ToLower().Split(" ");
            
            foreach ((var schoolName, var schoolId) in schools)
            {
                var found = splitText.All((key) =>
                {
                    return schoolName.ToLower().Contains(key.ToLower());
                });
                if (found)
                {
                    suitableItems.Add((schoolName, schoolId));
                }
            }
            if (suitableItems.Count == 0)
            {
                suitableItems.Add(("No results found", 0));
            }
            sender.ItemsSource = suitableItems;
        }
    }

    private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        //SuggestionOutput.Text = args.SelectedItem.ToString();
        LoginButton.IsEnabled = true;
        var selectedItem = args.SelectedItem as (string, int)?;
        selectedSchool = selectedItem.Value.Item2;
        sender.Text = selectedItem.Value.Item1;

    }

    private async void SchoolsList_OnFocus(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("TAPPED");
        if (schools == null || schools.Count <= 0)
        {
            await LoadSchoolIdsAsync();
        }
        var autoSuggestBox = sender as AutoSuggestBox;
        autoSuggestBox.ItemsSource = schools;
        if (autoSuggestBox.Text == "")
        {
            Debug.WriteLine("EMPTYTEXT");

            autoSuggestBox.IsSuggestionListOpen = true;
            autoSuggestBox.IsFocusEngaged = false;
        }
        else
        {
            autoSuggestBox.IsSuggestionListOpen = false;
        }

    }
}
