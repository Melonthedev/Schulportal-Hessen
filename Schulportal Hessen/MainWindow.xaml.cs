using Microsoft.UI.Xaml.Media;
using Windows.Storage;
using Schulportal_Hessen.Helpers;

using Windows.UI.ViewManagement;
using System.Diagnostics;
using Schulportal_Hessen.Views;
using Schulportal_Hessen.ViewModels;
using Schulportal_Hessen.Services;

namespace Schulportal_Hessen;

public sealed partial class MainWindow : WindowEx {
    private Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue;

    private UISettings settings;

    public MainWindow() {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();
        dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        settings = new UISettings();
        settings.ColorValuesChanged += Settings_ColorValuesChanged;
        //ApplySelectedBackdrop();
        //SettingsPage.BackdropChanged += SettingsPage_BackdropChanged;
        var SettingsService = App.GetService<SettingsService>();
        SettingsService.UpdateBackdrop += SettingsService_UpdateBackdrop;
    }

    private void SettingsService_UpdateBackdrop(object? sender, EventArgs e) => ApplySelectedBackdrop();

    // this handles updating the caption button colors correctly when indows system theme is changed
    // while the app is open
    private void Settings_ColorValuesChanged(UISettings sender, object args) {
        dispatcherQueue.TryEnqueue(TitleBarHelper.ApplySystemThemeToCaptionButtons);
    }

    private void ApplySelectedBackdrop() {
        var localSettings = ApplicationData.Current.LocalSettings;
        if (localSettings.Values["SystemBackdrop"] is not string selectedSystemBackdrop) return;
        SystemBackdrop = selectedSystemBackdrop.ToUpper() switch {
            "MICA" => new MicaBackdrop(),
            "ACRYLIC" => new DesktopAcrylicBackdrop(),
            "NONE" => null,
            "IMAGE" => new MicaBackdrop(),
            _ => null
        };
        Debug.WriteLine("Applying Backdrop: " + selectedSystemBackdrop);
    }
}
