using CommunityToolkit.Mvvm.ComponentModel;
using Schulportal_Hessen.Contracts.ViewModels;
using Schulportal_Hessen.Core.Contracts.Services;
using Schulportal_Hessen.Core.Models;
using System.Collections.ObjectModel;

namespace Schulportal_Hessen.ViewModels;

public partial class ChatsViewModel : ObservableRecipient, INavigationAware {
    private readonly ISampleDataService _sampleDataService;

    [ObservableProperty]
    private SampleOrder? selected;

    public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

    public ChatsViewModel(ISampleDataService sampleDataService) {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter) {
        SampleItems.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetListDetailsDataAsync();

        foreach (var item in data) {
            SampleItems.Add(item);
        }
    }

    public void OnNavigatedFrom() {
    }

    public void EnsureItemSelected() {
        Selected ??= SampleItems.First();
    }
}
