using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Schulportal_Hessen.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI;

namespace Schulportal_Hessen.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page {
    public SettingsViewModel ViewModel { get; }
    //public static event EventHandler? BackdropChanged;
    public Button currentColorSelectingButton;

    public SettingsPage() {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
        colorPickerButton.Background = new SolidColorBrush(ViewModel.PrimaryBackdropColor);
        primaryGradientColorPickerButton.Background = new SolidColorBrush(ViewModel.PrimaryGradientBackdropColor);
        secondaryGradientColorPickerButton.Background = new SolidColorBrush(ViewModel.SecondaryGradientBackdropColor);
    }

    private void confirmColor_Click(object sender, RoutedEventArgs e) {
        currentColorSelectingButton.Background = new SolidColorBrush(colorPicker.Color);
        switch (currentColorSelectingButton.Name) {
            case "colorPickerButton":
            default:
                ViewModel.PrimaryBackdropColor = colorPicker.Color;
                break;
            case "primaryGradientColorPickerButton":
                ViewModel.PrimaryGradientBackdropColor = colorPicker.Color;
                break;
            case "secondaryGradientColorPickerButton":
                ViewModel.SecondaryGradientBackdropColor = colorPicker.Color;
                break;
        }
        colorPickerButton.Flyout.Hide();
    }

    private void cancelColor_Click(object sender, RoutedEventArgs e) {
        colorPickerButton.Flyout.Hide();
    }

    private void colorPickerButton_Click(object sender, RoutedEventArgs e) {
        currentColorSelectingButton = (Button)sender;
        Debug.WriteLine(((Button)sender).Name);
        Color selectedColor;
        switch (((Button)sender).Name) {
            case "colorPickerButton":
            default:
                selectedColor = ViewModel.PrimaryBackdropColor;
                break;
            case "primaryGradientColorPickerButton":
                selectedColor = ViewModel.PrimaryGradientBackdropColor;
                break;
            case "secondaryGradientColorPickerButton":
                selectedColor = ViewModel.SecondaryGradientBackdropColor;
                break;
        }
        selectedColor.A += 255;
        colorPicker.Color = selectedColor;

    }

    private void SystemBackdropComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        backdropColorSettingsCard.Visibility = Visibility.Collapsed;
        backdropGradientColorSettingsCard.Visibility = Visibility.Collapsed;
        switch (SystemBackdropComboBox.SelectedValue) {
            case "MICA":
                break;
            case "ACRYLIC":
                break;
            case "SOLID":
                backdropColorSettingsCard.Visibility = Visibility.Visible;
                break;
            case "GRADIENT":
                backdropGradientColorSettingsCard.Visibility = Visibility.Visible;
                break;
            case "ACCENT":
                break;
            case "RAINBOW":
                break;
            case "IMAGE":
                break;
            case "NONE":
                break;
        }
    }

    /*private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
Debug.WriteLine("HELLO");
var localSettings = ApplicationData.Current.LocalSettings;
localSettings.Values["SystemBackdrop"] = ((ComboBoxItem) SystemBackdropComboBox.SelectedItem).Content.ToString().ToUpper();
BackdropChanged.Invoke(this, EventArgs.Empty);
}*/
}
