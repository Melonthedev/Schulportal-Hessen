using Windows.Storage;
using System.Diagnostics;
using Schulportal_Hessen.ViewModels;
using System.Drawing;

namespace Schulportal_Hessen.Services {
    public class SettingsService {

        public event EventHandler UpdateBackdrop;
        public ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;


        public SettingsService(SettingsViewModel settingsViewModel)
        {
            settingsViewModel.PropertyChanged += SettingsVM_PropertyChanged;
        }

        private void SettingsVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) {
            Debug.WriteLine($"[Settings] Property {e.PropertyName} Changed");
            switch (e.PropertyName) {
                case "Backdrop":
                case "PrimaryBackdropColor":
                case "PrimaryGradientBackdropColor":
                case "SecondaryGradientBackdropColor":
                    UpdateBackdrop?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }


        public ApplicationDataCompositeValue GetComposite(string key) {
            return (ApplicationDataCompositeValue)LocalSettings.Values[key];
        }

    }
}
