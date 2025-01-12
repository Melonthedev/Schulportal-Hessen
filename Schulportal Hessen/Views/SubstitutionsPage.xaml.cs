using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Schulportal_Hessen.ViewModels;

namespace Schulportal_Hessen.Views;

public sealed partial class SubstitutionsPage : Page {
    private int previousSelectedIndex;

    public SubstitutionsViewModel ViewModel {
        get;
    }

    public SubstitutionsPage() {
        ViewModel = App.GetService<SubstitutionsViewModel>();
        InitializeComponent();
    }

    private void SelectorBar2_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args) {
        SelectorBarItem selectedItem = sender.SelectedItem;
        int currentSelectedIndex = sender.Items.IndexOf(selectedItem);
        System.Type pageType;

        switch (currentSelectedIndex) {
            case 0:
                pageType = typeof(FirstSubstitutionsPage);

                break;
            case 1:
                pageType = typeof(SecondSubstitutionsPage);
                break;
            default:
                pageType = typeof(FirstSubstitutionsPage);
                break;
        }

        var slideNavigationTransitionEffect = currentSelectedIndex - previousSelectedIndex > 0 ? SlideNavigationTransitionEffect.FromRight : SlideNavigationTransitionEffect.FromLeft;

        ContentFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = slideNavigationTransitionEffect });

        previousSelectedIndex = currentSelectedIndex;

    }
}
