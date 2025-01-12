using Schulportal_Hessen.Core.Models;

namespace Schulportal_Hessen.Core.Contracts.Services;

// Remove this class once your pages/features are using your data.
public interface ISampleDataService {
    Task<IEnumerable<SampleOrder>> GetContentGridDataAsync();

    Task<IEnumerable<SampleOrder>> GetListDetailsDataAsync();

    Task<IEnumerable<SampleOrder>> GetGridDataAsync();
}
