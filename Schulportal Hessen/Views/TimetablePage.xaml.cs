using Microsoft.UI.Xaml.Controls;

using Schulportal_Hessen.ViewModels;

namespace Schulportal_Hessen.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class TimetablePage : Page
{
    public TimetableViewModel ViewModel
    {
        get;
    }

    public TimetablePage()
    {
        ViewModel = App.GetService<TimetableViewModel>();
        InitializeComponent();
    }
}
