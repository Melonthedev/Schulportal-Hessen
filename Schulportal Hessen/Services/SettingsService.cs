using Windows.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schulportal_Hessen.ViewModels;

namespace Schulportal_Hessen.Services {
    public class SettingsService {

        public event EventHandler UpdateBackdrop;
        public ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;


        public SettingsService(SettingsViewModel settingsViewModel)
        {
            settingsViewModel.PropertyChanged += SettingsVM_PropertyChanged;
        }

        private void SettingsVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) {
            Debug.WriteLine("[Settings] Property Changed");
            switch (e.PropertyName) {
                case "Backdrop":
                    UpdateBackdrop?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }


    }
}
