using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using Schulportal_Hessen.Contracts.Services;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Schulportal_Hessen.Services;

public class WebViewService : IWebViewService {
    private WebView2? _webView;

    public Uri? Source => _webView?.Source;

    [MemberNotNullWhen(true, nameof(_webView))]
    public bool CanGoBack => _webView != null && _webView.CanGoBack;

    [MemberNotNullWhen(true, nameof(_webView))]
    public bool CanGoForward => _webView != null && _webView.CanGoForward;

    public event EventHandler<CoreWebView2WebErrorStatus>? NavigationCompleted;

    private readonly AuthService _authService;

    public WebViewService(AuthService authService) {
        _authService = authService;
    }

    [MemberNotNull(nameof(_webView))]
    public async void Initialize(WebView2 webView) {
        _webView = webView;
        _webView.NavigationCompleted += OnWebViewNavigationCompleted;
        await _webView.EnsureCoreWebView2Async();
        //Debug.WriteLine("WEBVIEWCOREREADDY");
        _webView.Source = new("https://start.schulportal.hessen.de/index.php");
        var sessionCookie = _authService.SPHSession;
        if (string.IsNullOrEmpty(sessionCookie)) return;
        _webView.CoreWebView2.CookieManager.CreateCookie("SPH-Session", sessionCookie, ".hessen.de", "/");
        _webView.CoreWebView2.OpenDevToolsWindow();
    }

    public void GoBack() => _webView?.GoBack();

    public void GoForward() => _webView?.GoForward();

    public void Reload() => _webView?.Reload();

    public void UnregisterEvents() {
        if (_webView != null) {
            _webView.NavigationCompleted -= OnWebViewNavigationCompleted;
        }
    }

    private void OnWebViewNavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args) => NavigationCompleted?.Invoke(this, args.WebErrorStatus);
}
