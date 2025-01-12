using Microsoft.UI.Xaml.Media;
using Windows.Storage;
using Schulportal_Hessen.Helpers;

using Windows.UI.ViewManagement;
using System.Diagnostics;
using Schulportal_Hessen.Views;

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
        ApplySelectedBackdrop();
        SettingsPage.BackdropChanged += SettingsPage_BackdropChanged;
    }

    private void SettingsPage_BackdropChanged(object? sender, EventArgs e) => ApplySelectedBackdrop();

    // this handles updating the caption button colors correctly when indows system theme is changed
    // while the app is open
    private void Settings_ColorValuesChanged(UISettings sender, object args) {
        dispatcherQueue.TryEnqueue(TitleBarHelper.ApplySystemThemeToCaptionButtons);
    }

    private void ApplySelectedBackdrop() {
        var localSettings = ApplicationData.Current.LocalSettings;
        var selectedSystemBackdrop = localSettings.Values["SystemBackdrop"] as string;
        Debug.WriteLine(selectedSystemBackdrop);
        if (selectedSystemBackdrop is null) return;
        switch (selectedSystemBackdrop.ToUpper()) {
            case "MICA":
                this.SystemBackdrop = new MicaBackdrop();
                break;
            case "ACRYLIC":
                this.SystemBackdrop = new DesktopAcrylicBackdrop();
                break;
        }
    }
}
