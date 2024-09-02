using Microsoft.UI.Xaml.Controls;

using Schulportal_Hessen.ViewModels;

namespace Schulportal_Hessen.Views;

// To learn more about WebView2, see https://docs.microsoft.com/microsoft-edge/webview2/.
public sealed partial class WebansichtPage : Page
{
    public WebansichtViewModel ViewModel
    {
        get;
    }

    public WebansichtPage()
    {
        ViewModel = App.GetService<WebansichtViewModel>();
        InitializeComponent();

        ViewModel.WebViewService.Initialize(WebView);
    }
}
