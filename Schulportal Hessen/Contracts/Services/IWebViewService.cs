﻿using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

namespace Schulportal_Hessen.Contracts.Services;

public interface IWebViewService {
    Uri? Source {
        get;
    }

    bool CanGoBack {
        get;
    }

    bool CanGoForward {
        get;
    }

    event EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2WebErrorStatus>? NavigationCompleted;

    void Initialize(WebView2 webView);

    void GoBack();

    void GoForward();

    void Reload();

    void UnregisterEvents();
}
