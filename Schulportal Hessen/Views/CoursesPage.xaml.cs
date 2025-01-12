using Microsoft.UI.Xaml.Controls;

using Schulportal_Hessen.ViewModels;

namespace Schulportal_Hessen.Views;

public sealed partial class CoursesPage : Page {
    public CoursesViewModel ViewModel { get; }

    public CoursesPage() {
        ViewModel = App.GetService<CoursesViewModel>();
        InitializeComponent();
    }
}
