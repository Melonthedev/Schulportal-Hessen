using CommunityToolkit.WinUI.UI.Animations;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using Schulportal_Hessen.Contracts.Services;
using Schulportal_Hessen.ViewModels;

namespace Schulportal_Hessen.Views;

public sealed partial class InhaltsrasterDetailPage : Page
{
    public InhaltsrasterDetailViewModel ViewModel
    {
        get;
    }

    public InhaltsrasterDetailPage()
    {
        ViewModel = App.GetService<InhaltsrasterDetailViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("animationKeyContentGrid", itemHero);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (e.NavigationMode == NavigationMode.Back)
        {
            var navigationService = App.GetService<INavigationService>();

            if (ViewModel.Item != null)
            {
                navigationService.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }
}
