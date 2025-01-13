using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Schulportal_Hessen.Contracts.Services;
using Schulportal_Hessen.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Schulportal_Hessen.ViewModels;

public partial class SettingsViewModel : ObservableRecipient, INotifyPropertyChanged {
    private readonly IThemeSelectorService _themeSelectorService;

    private readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

    private ElementTheme _elementTheme;

    public ElementTheme ElementTheme {
        get => _elementTheme;
        set {
            if (_elementTheme != value) {
                _elementTheme = value;

                _themeSelectorService.SetThemeAsync(value);
                OnPropertyChanged(nameof(ElementTheme));
            }
        }
    }

    [ObservableProperty]
    private string _versionDescription;

    [ObservableProperty]
    private string _configVersionDescription;


    private string _backdrop = "MICA";

    public string Backdrop {
        get => _backdrop;
        set {
            if (_backdrop != value) {
                _backdrop = value;

                LocalSettings.Values["SystemBackdrop"] = value;
                OnPropertyChanged(nameof(Backdrop));
            }
        }
    }


    public ICommand SwitchThemeCommand {
        get;
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService) {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();
        _backdrop = (string) LocalSettings.Values["SystemBackdrop"];
    }

    private static string GetVersionDescription() {
        Version version;

        if (RuntimeHelper.IsMSIX) {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        } else {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
