using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Schulportal_Hessen.Contracts.Services;
using Schulportal_Hessen.Contracts.ViewModels;
using Schulportal_Hessen.Core.Contracts.Services;
using Schulportal_Hessen.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Schulportal_Hessen.ViewModels;

public partial class InhaltsrasterViewModel : ObservableRecipient, INavigationAware {
    private readonly INavigationService _navigationService;
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

    public InhaltsrasterViewModel(INavigationService navigationService, ISampleDataService sampleDataService) {
        _navigationService = navigationService;
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter) {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetContentGridDataAsync();
        foreach (var item in data) {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom() {
    }

    [RelayCommand]
    private void OnItemClick(SampleOrder? clickedItem) {
        if (clickedItem != null) {
            _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
            _navigationService.NavigateTo(typeof(InhaltsrasterDetailViewModel).FullName!, clickedItem.OrderID);
        }
    }
}
