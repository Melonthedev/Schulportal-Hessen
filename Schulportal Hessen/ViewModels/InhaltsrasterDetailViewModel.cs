using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Animation;
using Schulportal_Hessen.Contracts.ViewModels;
using Schulportal_Hessen.Core.Contracts.Services;
using Schulportal_Hessen.Core.Models;

namespace Schulportal_Hessen.ViewModels;

public partial class InhaltsrasterDetailViewModel : ObservableRecipient, INavigationAware {
    private readonly ISampleDataService _sampleDataService;

    [ObservableProperty]
    private SampleOrder? item;

    public InhaltsrasterDetailViewModel(ISampleDataService sampleDataService) {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter) {
        if (parameter is long orderID) {
            var data = await _sampleDataService.GetContentGridDataAsync();
            Item = data.First(i => i.OrderID == orderID);
        }
    }

    public void OnNavigatedFrom() {
    }
}
