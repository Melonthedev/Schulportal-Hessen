using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Schulportal_Hessen.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Storage;

namespace Schulportal_Hessen.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page {
    public SettingsViewModel ViewModel { get; }
    //public static event EventHandler? BackdropChanged;

    public SettingsPage() {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    private void confirmColor_Click(object sender, RoutedEventArgs e) {
        // Assign the selected color to a variable to use outside the popup.
        ViewModel.Color = colorPicker.Color;
        colorPickerButton.Background = new SolidColorBrush(ViewModel.Color);

        // Close the Flyout.
        colorPickerButton.Flyout.Hide();
    }

    private void cancelColor_Click(object sender, RoutedEventArgs e) {
        // Close the Flyout.
        colorPickerButton.Flyout.Hide();
    }
    /*private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        Debug.WriteLine("HELLO");
        var localSettings = ApplicationData.Current.LocalSettings;
        localSettings.Values["SystemBackdrop"] = ((ComboBoxItem) SystemBackdropComboBox.SelectedItem).Content.ToString().ToUpper();
        BackdropChanged.Invoke(this, EventArgs.Empty);
    }*/
}
