﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Web.WebView2.Core;

using Schulportal_Hessen.Contracts.Services;
using Schulportal_Hessen.Contracts.ViewModels;

namespace Schulportal_Hessen.ViewModels;

// TODO: Review best practices and distribution guidelines for WebView2.
// https://docs.microsoft.com/microsoft-edge/webview2/get-started/winui
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/developer-guide
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/distribution
public partial class WebansichtViewModel : ObservableRecipient, INavigationAware {
    // TODO: Set the default URL to display.
    [ObservableProperty]
    private Uri source = new("https://start.schulportal.hessen.de/index.php");

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private bool hasFailures;

    public IWebViewService WebViewService {
        get;
    }

    public WebansichtViewModel(IWebViewService webViewService) {
        WebViewService = webViewService;
    }

    [RelayCommand]
    private async Task OpenInBrowser() {
        if (WebViewService.Source != null) {
            await Windows.System.Launcher.LaunchUriAsync(WebViewService.Source);
        }
    }

    [RelayCommand]
    private void Reload() {
        WebViewService.Reload();
    }

    [RelayCommand(CanExecute = nameof(BrowserCanGoForward))]
    private void BrowserForward() {
        if (WebViewService.CanGoForward) {
            WebViewService.GoForward();
        }
    }

    private bool BrowserCanGoForward() {
        return WebViewService.CanGoForward;
    }

    [RelayCommand(CanExecute = nameof(BrowserCanGoBack))]
    private void BrowserBack() {
        if (WebViewService.CanGoBack) {
            WebViewService.GoBack();
        }
    }

    private bool BrowserCanGoBack() {
        return WebViewService.CanGoBack;
    }

    public void OnNavigatedTo(object parameter) {
        WebViewService.NavigationCompleted += OnNavigationCompleted;
    }

    public void OnNavigatedFrom() {
        WebViewService.UnregisterEvents();
        WebViewService.NavigationCompleted -= OnNavigationCompleted;
    }

    private void OnNavigationCompleted(object? sender, CoreWebView2WebErrorStatus webErrorStatus) {
        IsLoading = false;
        BrowserBackCommand.NotifyCanExecuteChanged();
        BrowserForwardCommand.NotifyCanExecuteChanged();

        if (webErrorStatus != default) {
            HasFailures = true;
        }
    }

    [RelayCommand]
    private void OnRetry() {
        HasFailures = false;
        IsLoading = true;
        WebViewService?.Reload();
    }
}
