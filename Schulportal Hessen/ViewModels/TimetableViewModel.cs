using CommunityToolkit.Mvvm.ComponentModel;
using Schulportal_Hessen.Contracts.ViewModels;
using Schulportal_Hessen.Core.Contracts.Services;
using Schulportal_Hessen.Core.Models;
using System.Collections.ObjectModel;

namespace Schulportal_Hessen.ViewModels;

public partial class TimetableViewModel : ObservableRecipient, INavigationAware {
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

    public TimetableViewModel(ISampleDataService sampleDataService) {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter) {
        Source.Clear();

        // TODO: Replace with real data.
        /*var data = await _sampleDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }*/
    }

    public void OnNavigatedFrom() {
    }
}
