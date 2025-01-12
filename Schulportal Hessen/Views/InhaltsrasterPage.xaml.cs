using Microsoft.UI.Xaml.Controls;

using Schulportal_Hessen.ViewModels;

namespace Schulportal_Hessen.Views;

public sealed partial class InhaltsrasterPage : Page {
    public InhaltsrasterViewModel ViewModel { get; }

    public InhaltsrasterPage() {
        ViewModel = App.GetService<InhaltsrasterViewModel>();
        InitializeComponent();
    }
}
