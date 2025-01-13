using Microsoft.UI.Xaml.Controls;

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


    /*private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        Debug.WriteLine("HELLO");
        var localSettings = ApplicationData.Current.LocalSettings;
        localSettings.Values["SystemBackdrop"] = ((ComboBoxItem) SystemBackdropComboBox.SelectedItem).Content.ToString().ToUpper();
        BackdropChanged.Invoke(this, EventArgs.Empty);
    }*/
}
