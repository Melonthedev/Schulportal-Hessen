
using Microsoft.UI.Xaml.Controls;

using Schulportal_Hessen.ViewModels;

namespace Schulportal_Hessen.Views;

public sealed partial class ChatsPage : Page {
    public ChatsViewModel ViewModel { get; }

    public ChatsPage() {
        ViewModel = App.GetService<ChatsViewModel>();
        InitializeComponent();
    }

    /*private void OnViewStateChanged(object sender, ListDetailsViewState e) {
        if (e == ListDetailsViewState.Both) {
            ViewModel.EnsureItemSelected();
        }
    }*/
}
