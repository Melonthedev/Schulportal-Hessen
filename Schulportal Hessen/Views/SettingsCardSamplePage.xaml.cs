using Microsoft.UI.Xaml.Controls;

using Schulportal_Hessen.ViewModels;

namespace Schulportal_Hessen.Views;

public sealed partial class SettingsCardSamplePage : Page
{
    public SettingsCardSampleViewModel ViewModel
    {
        get;
    }

    public SettingsCardSamplePage()
    {
        ViewModel = App.GetService<SettingsCardSampleViewModel>();
        InitializeComponent();
    }
}
