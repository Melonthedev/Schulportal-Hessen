using Microsoft.UI.Xaml.Controls;

using Schulportal_Hessen.ViewModels;

namespace Schulportal_Hessen.Views;

public sealed partial class SecondSubstitutionsPage : Page
{
    public SecondSubstitutionsViewModel ViewModel
    {
        get;
    }

    public SecondSubstitutionsPage()
    {
        ViewModel = App.GetService<SecondSubstitutionsViewModel>();
        InitializeComponent();
    }
}
