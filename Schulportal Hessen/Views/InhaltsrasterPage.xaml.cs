using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Schulportal_Hessen.Core.Models;
using Schulportal_Hessen.ViewModels;
using System.Diagnostics;
namespace Schulportal_Hessen.Views;

public sealed partial class InhaltsrasterPage : Page {
    public InhaltsrasterViewModel ViewModel { get; }

    public InhaltsrasterPage() {
        ViewModel = App.GetService<InhaltsrasterViewModel>();
        InitializeComponent();
    }

    public void ItemClick(object sender, ItemClickEventArgs e) {
        if (e.ClickedItem is SampleOrder clickedItem) {
            if (clickedItem != null) {
                ViewModel._navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
                ViewModel._navigationService.NavigateTo(typeof(InhaltsrasterDetailViewModel).FullName!, clickedItem.OrderID);
            }
        }
    }

}
