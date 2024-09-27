using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

using Schulportal_Hessen.Contracts.Services;
using Schulportal_Hessen.Helpers;
using Schulportal_Hessen.ViewModels;

using Windows.System;

namespace Schulportal_Hessen.Views;

public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel
    {
        get;
    }
    public SpWrapper _SpWrapper
    {
        get;
    }

    public static ShellPage Instance { get; private set; }

    public ShellPage(ShellViewModel viewModel, SpWrapper spWrapper)
    {
        Instance = this;
        ViewModel = viewModel;
        _SpWrapper = spWrapper;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);

        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();
    }

    private async void OnLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
        await LoadContents();
    }

    public async Task LoadContents()
    {
        Debug.WriteLine("LoadedddShellpaghe");
        if (!await _SpWrapper.AutoLoginAsync())
        {
            return;
        }
        UpdateLoginStatusUi();
    }

    public async void UpdateLoginStatusUi()
    {
        if (_SpWrapper.GetAuthService().isLoggedIn)
        {
        
            LoginItem.Visibility = Visibility.Collapsed;
            AccountItem.Visibility = Visibility.Visible;
            AccountItem.Content = await _SpWrapper.GetFullNameAsync();
            AccountFlyoutNameText.Text = await _SpWrapper.GetFullNameAsync() + " (" + await _SpWrapper.GetSchoolClassAsync() + ")";
            AccountFlyoutLogoutButton.Click += async (sender, e) =>
            {
                await _SpWrapper.LogoutAsync();
                NavigationFrame.Navigate(typeof(LoginPage));
                NavigationViewControl.Header = "Login";
                AccountFlyout.Hide();
                UpdateLoginStatusUi();
            };
        }
        else
        {
            LoginItem.Visibility = Visibility.Visible;
            AccountItem.Visibility = Visibility.Collapsed;
        }
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        App.AppTitlebar = AppTitleBarText as UIElement;
    }

    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBar.Margin = new Thickness()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }

    private void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
    {
        if (AccountFlyout != null)
        {
            AccountFlyout.ShowAt(AccountItem);
        }
    }

    private void LoginItem_Tapped(object sender, TappedRoutedEventArgs e)
    {
        NavigationFrame.Navigate(typeof(LoginPage));    
        NavigationViewControl.Header = "Login";
    }
}
