using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Schulportal_Hessen.Helpers;
using Schulportal_Hessen.ViewModels;

namespace Schulportal_Hessen.Views;

public sealed partial class TimetablePage : Page
{
    public TimetableViewModel ViewModel
    {
        get;
    }

    public SpWrapper _SpWrapper
    {
        get;
    }

    public TimetablePage()
    {
        ViewModel = App.GetService<TimetableViewModel>();
        _SpWrapper = App.GetService<SpWrapper>();
        Loaded += TimetablePage_Loaded;
        InitializeComponent();
    }


    private async void TimetablePage_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadContents();
    }

    public async Task LoadContents()
    {
        if (!await _SpWrapper.AutoLoginAsync())
        {
            return;
        }
        TimetableHeader.Text = await _SpWrapper.GetSchoolClassAsync();
        _SpWrapper.GetTimetableAsync();
    }
}
