using Microsoft.UI.Xaml.Controls;

using Schulportal_Hessen.ViewModels;

namespace Schulportal_Hessen.Views;

public sealed partial class FirstSubstitutionsPage : Page
{
    public FirstSubstitutionsViewModel ViewModel
    {
        get;
    }

    public FirstSubstitutionsPage()
    {
        ViewModel = App.GetService<FirstSubstitutionsViewModel>();
        InitializeComponent();
    }
}
