using System.Diagnostics;
using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Schulportal_Hessen.Contracts.Services;
using Schulportal_Hessen.Helpers;
using Schulportal_Hessen.Services;
using Schulportal_Hessen.ViewModels;
using Windows.Foundation;
using Windows.Graphics;
using Windows.System;
using WinRT.Interop;

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

    private readonly AppWindow m_AppWindow;
    private readonly ErrorService _errorService;
    private readonly NetworkService _networkService;
    private readonly AuthService _authService;

    public ShellPage(ShellViewModel viewModel, SpWrapper spWrapper, ErrorService errorService, NetworkService networkService, AuthService authService)
    {
        ViewModel = viewModel;
        _SpWrapper = spWrapper;
        _errorService = errorService;
        _networkService = networkService;
        _authService = authService;
        m_AppWindow = App.MainWindow.AppWindow;

        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);

        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();

        AppTitleBar.Loaded += AppTitleBar_Loaded;
        AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;
        _errorService.OnErrorOccurred += ShowError;
        _networkService.OnConnectionStatusChanged += NetworkService_OnConnectionStatusChanged;
        _authService.OnLoggedIn += AuthService_OnLoggedIn;
    }


    public void ShowError(string title, string message)
        => ShowInformation(title, message, InfoBarSeverity.Error, false, true);

    public void ShowSuccess(string title, string message) 
        => ShowInformation(title, message, InfoBarSeverity.Success, false, true);

    public async Task ShowInformation(string title, string message, InfoBarSeverity severity, bool closable, bool autoClose)
    {
        // Open
        InformationBar.Opacity = 1;
        InformationBar.Visibility = Visibility.Visible;
        InformationBar.IsOpen = true;
        // Set
        InformationBar.Message = message;
        InformationBar.Title = title;
        InformationBar.IsClosable = closable;
        InformationBar.Severity = severity;
        // Close
        if (!autoClose) return;
        await Task.Delay(5000);
        FadeOutStoryboard.Begin();
        FadeOutStoryboard.Completed += (sender, e) => InformationBar.IsOpen = false;
    }        

    public void HideInfoBar()
    {
        InformationBar.IsOpen = false;
    }

    public void NetworkService_OnConnectionStatusChanged(bool IsOffline)
    {
        Debug.WriteLine("Offline: " + IsOffline);
        if (IsOffline) 
        {
            ConnectionStatusButton.Visibility = Visibility.Visible;
        }
        else
        {
            ConnectionStatusButton.Visibility = Visibility.Collapsed;
            HideInfoBar();
            ShowSuccess("Reconnected", "You are back online");
        }
        if (App.MainWindow.ExtendsContentIntoTitleBar == true)
        {
            // Adjust the regions for the custom title bar
            try
            {
                //Debug.WriteLine("ANPASSEN WEGEN NEU ICON KEIN INTERNET");
                SetRegionsForCustomTitleBar();
            } catch (Exception) { }
        }
    }

    private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
    {
        if (App.MainWindow.ExtendsContentIntoTitleBar == true)
        {
            SetRegionsForCustomTitleBar();
        }
    }

    private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (App.MainWindow.ExtendsContentIntoTitleBar == true)
        {
            SetRegionsForCustomTitleBar();

            // Only show the fitted connection status button if the user really is offline
            if (!_networkService.IsOffline) return; 
            if (AppTitleBar.ActualWidth < 700)
            {
                ConnectionStatusButtonText.Visibility = Visibility.Collapsed;
            }
            else
            {
                ConnectionStatusButtonText.Visibility = Visibility.Visible;
            }
            if (AppTitleBar.ActualWidth < 600)
            {
                ConnectionStatusButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                ConnectionStatusButton.Visibility = Visibility.Visible;
            }
        }
    }

    private void SetRegionsForCustomTitleBar()
    {
        if (AppTitleBar.XamlRoot == null) return;
        var scaleAdjustment = AppTitleBar.XamlRoot.RasterizationScale;
        RightPaddingColumn.Width = new GridLength(m_AppWindow.TitleBar.RightInset / scaleAdjustment);
        LeftPaddingColumn.Width = new GridLength(m_AppWindow.TitleBar.LeftInset / scaleAdjustment);
        //Debug.WriteLine(ConnectionStatusButton.ActualWidth);
        GeneralTransform transform = ConnectionStatusButton.TransformToVisual(null);
        Rect bounds = transform.TransformBounds(new Rect(0, 0,
                                                         ConnectionStatusButton.ActualWidth,
                                                         ConnectionStatusButton.ActualHeight));
        RectInt32 ConnectionStatusButtonRect = GetRect(bounds, scaleAdjustment);
        //Debug.WriteLine(ConnectionStatusButtonRect.Width);
        //Debug.WriteLine(ConnectionStatusButtonRect.X);
        //Debug.WriteLine(ConnectionStatusButton.TransformToVisual(App.MainWindow.Content).TransformPoint(new Point(0, 0)).X);
        //Debug.WriteLine($"Bounds: X={bounds.X}, Y={bounds.Y}, Width={bounds.Width}, Height={bounds.Height}");
        //Debug.WriteLine($"Passthrough Region X: {ConnectionStatusButtonRect.X}, Y: {ConnectionStatusButtonRect.Y}, Width: {ConnectionStatusButtonRect.Width}, Height: {ConnectionStatusButtonRect.Height}");

        /*transform = PersonPic.TransformToVisual(null);
        bounds = transform.TransformBounds(new Rect(0, 0,
                                                    PersonPic.ActualWidth,
                                                    PersonPic.ActualHeight));
        Windows.Graphics.RectInt32 PersonPicRect = GetRect(bounds, scaleAdjustment);*/

        var rectArray = new RectInt32[] { ConnectionStatusButtonRect };
        var nonClientInputSrc = InputNonClientPointerSource.GetForWindowId(m_AppWindow.Id);
        //Debug.WriteLine("OEBUS");
        nonClientInputSrc.SetRegionRects(NonClientRegionKind.Passthrough, rectArray);
    }


    private RectInt32 GetRect(Rect bounds, double scale)
    {
        return new RectInt32(
            _X: (int)Math.Round(bounds.X * scale),
            _Y: (int)Math.Round(bounds.Y * scale),
            _Width: (int)Math.Round(bounds.Width * scale),
            _Height: (int)Math.Round(bounds.Height * scale)
        );
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
        await LoadContents();
    }

    public async Task LoadContents()
    {
        await _SpWrapper.AutoLoginAsync();
    }

    public async void UpdateLoginStatusUi()
    {
        if (_authService.isLoggedIn)
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

    private void ConnectionStatusFlyoutReconnectButton_Click(object sender, RoutedEventArgs e)
    {
        // Reconnect to the internet
        _SpWrapper.PingSchulportal();
        ConnectionStatusFlyout.Hide();
    }

    private void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
    {
        AccountFlyout?.ShowAt(AccountItem);
    }

    private void LoginItem_Tapped(object sender, TappedRoutedEventArgs e)
    {
        NavigationFrame.Navigate(typeof(LoginPage));
        NavigationViewControl.Header = "Login";
    }

    public void AuthService_OnLoggedIn()
    {
        UpdateLoginStatusUi();
        ShowSuccess("Success", "Logged in successfully");
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

    
}
