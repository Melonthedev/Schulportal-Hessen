using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Schulportal_Hessen.Core.Models;

namespace Schulportal_Hessen.Views;

public sealed partial class ChatsControl : UserControl {
    public SampleOrder? ChatsMenuItem {
        get => GetValue(ChatsMenuItemProperty) as SampleOrder;
        set => SetValue(ChatsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ChatsMenuItemProperty = DependencyProperty.Register("ChatsMenuItem", typeof(SampleOrder), typeof(ChatsControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public ChatsControl() {
        InitializeComponent();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is ChatsControl control) {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
