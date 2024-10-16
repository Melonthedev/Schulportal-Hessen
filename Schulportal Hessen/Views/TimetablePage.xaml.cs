using CommunityToolkit.WinUI.UI;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Schulportal_Hessen.Helpers;
using Schulportal_Hessen.Models;
using Schulportal_Hessen.Services;
using Schulportal_Hessen.ViewModels;
using System.Diagnostics;
using Windows.UI;

namespace Schulportal_Hessen.Views;

public sealed partial class TimetablePage : Page {

    public TimetableViewModel ViewModel { get; }
    public SpWrapper _SpWrapper { get; }
    public TimeTableService _TimeTableService { get; }

    public TimetablePage() {
        ViewModel = App.GetService<TimetableViewModel>();
        _SpWrapper = App.GetService<SpWrapper>();
        _TimeTableService = App.GetService<TimeTableService>();
        Loaded += TimetablePage_Loaded;
        InitializeComponent();
    }


    private async void TimetablePage_Loaded(object sender, RoutedEventArgs e) {
        await LoadContents();
    }

    public async Task LoadContents() {
        if (!await _SpWrapper.AutoLoginAsync()) {
            return;
        }
        TimetableHeader.Text = await _SpWrapper.GetSchoolClassAsync();
        var timeTableLessons = await _SpWrapper.GetTimetableAsync();
        foreach (var lesson in timeTableLessons) {
            var border = new Border();
            border.Background = GetRandomSolidColorBrush(); //TODO ANPASSBARE FÄCHER FABEN DIE ALLE GLEICHEN FÄCHER IN EINER FARBE EINFÄRBEN
            border.CornerRadius = new CornerRadius(5);
            border.Padding = new Thickness(10);
            border.Margin = new Thickness(5);

            var container = new StackPanel() {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var lessonName = new TextBlock {
                Text = lesson.Subject,
                FontSize = 18,
                FontWeight = FontWeights.SemiBold,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            container.Children.Add(lessonName);

            var roomContainer = new StackPanel() {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var roomFontIcon = new FontIcon() {
                Glyph = "\uE77B",
                FontSize = 10,
                Margin = new Thickness(0, 0, 5, 0)
            };
            roomContainer.Children.Add(roomFontIcon);
            var lessonRoom = new TextBlock {
                Text = lesson.Room,
                FontSize = 15,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            roomContainer.Children.Add(lessonRoom);

            var teacherContainer = new StackPanel() {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var teacherFontIcon = new FontIcon() {
                Glyph = "\uE816",
                FontSize = 10,
                Margin = new Thickness(0, 0, 5, 0)
            };
            teacherContainer.Children.Add(teacherFontIcon);
            var lessonTeacher = new TextBlock {
                Text = lesson.Teacher,
                FontSize = 15,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            teacherContainer.Children.Add(lessonTeacher);

            container.Children.Add(teacherContainer);
            container.Children.Add(roomContainer);
            border.Child = container;
            var hour = lesson.Hour >= 3 ? lesson.Hour + 1 : lesson.Hour;
            Debug.WriteLine(hour);
            Grid.SetRow(border, hour);
            Grid.SetColumn(border, lesson.Day);
            TimeTableGrid.Children.Add(border);
            // TODO: HANDLE SEVERAL TIMETABLELESSONS ON SAME GRID POS

        }
    }

    public SolidColorBrush GetRandomSolidColorBrush() {
        Random random = new Random();
        // Generiere zufällige Werte für Rot, Grün und Blau
        byte r = (byte)random.Next(256);
        byte g = (byte)random.Next(256);
        byte b = (byte)random.Next(256);

        // Erstelle die SolidColorBrush mit der zufälligen Farbe
        Color randomColor = Color.FromArgb(255, r, g, b); // 255 für volle Deckkraft
        return new SolidColorBrush(randomColor);
    }
}
