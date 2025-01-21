using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Schulportal_Hessen.Contracts.Services;
using Schulportal_Hessen.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI;

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

    private Color _primaryBackdropColor;

    public Color PrimaryBackdropColor {
        get => _primaryBackdropColor;
        set {
            if (_primaryBackdropColor != value) {
                _primaryBackdropColor = value;

                LocalSettings.Values["PrimaryBackdropColor"] = ColorHelper.ToHex(value);
                OnPropertyChanged(nameof(PrimaryBackdropColor));
            }
        }
    }

    private Color _primaryGradientBackdropColor;

    public Color PrimaryGradientBackdropColor {
        get => _primaryGradientBackdropColor;
        set {
            if (_primaryGradientBackdropColor != value) {
                _primaryGradientBackdropColor = value;

                LocalSettings.Values["PrimaryGradientBackdropColor"] = ColorHelper.ToHex(value);
                OnPropertyChanged(nameof(PrimaryGradientBackdropColor));
            }
        }
    }

    private Color _secondaryGradientBackdropColor;

    public Color SecondaryGradientBackdropColor {
        get => _secondaryGradientBackdropColor;
        set {
            if (_secondaryGradientBackdropColor != value) {
                _secondaryGradientBackdropColor = value;

                LocalSettings.Values["SecondaryGradientBackdropColor"] = ColorHelper.ToHex(value);
                OnPropertyChanged(nameof(SecondaryGradientBackdropColor));
            }
        }
    }

    private string _backdrop;

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
        _primaryBackdropColor = LocalSettings.Values["PrimaryBackdropColor"] is null ? Microsoft.UI.Colors.DarkBlue : ColorHelper.ToColor((string)LocalSettings.Values["PrimaryBackdropColor"]);
        _primaryGradientBackdropColor = LocalSettings.Values["PrimaryGradientBackdropColor"] is null ? Microsoft.UI.Colors.OrangeRed : ColorHelper.ToColor((string)LocalSettings.Values["PrimaryGradientBackdropColor"]);
        _secondaryGradientBackdropColor = LocalSettings.Values["SecondaryGradientBackdropColor"] is null ? Microsoft.UI.Colors.DarkBlue : ColorHelper.ToColor((string)LocalSettings.Values["SecondaryGradientBackdropColor"]);
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
