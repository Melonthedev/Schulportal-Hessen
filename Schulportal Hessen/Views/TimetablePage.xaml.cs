using CommunityToolkit.WinUI;
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
    public bool InEditMode { get; set; }

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

        await _TimeTableService.SyncWithSchulPortal();

        var timeTableLessons = _TimeTableService.GetLessons();
        foreach (var lesson in timeTableLessons) {
            var border = new Border();
            border.Background = new SolidColorBrush(lesson.Course.Color);
            border.CornerRadius = new CornerRadius(5);
            border.Padding = new Thickness(10);
            //border.Margin = new Thickness(5);
            border.VerticalAlignment = VerticalAlignment.Stretch;

            var container = new StackPanel() {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var lessonName = new TextBlock {
                Text = lesson.Course.Abbreviation,
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
                Text = lesson.Course.Teacher,
                FontSize = 15,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            teacherContainer.Children.Add(lessonTeacher);

            container.Children.Add(teacherContainer);
            container.Children.Add(roomContainer);
            border.Child = container;
            AddLessonToGrid(lesson, border);
        }

        EditButton.Click += EditButton_Click;
        ExitEditButton.Click += ExitEditButton_Click;
    }

    private void ExitEditButton_Click(object sender, RoutedEventArgs e) {
        InEditMode = false;
    }

    private async void EditButton_Click(object sender, RoutedEventArgs e) {
        InEditMode = true;
        ContentDialogResult result = await editCourseDialog.ShowAsync();
    }

    public void AddLessonToGrid(TimeTableLesson lesson, Border border) {
        var hour = lesson.Hour >= 3 ? lesson.Hour + 1 : lesson.Hour;
        var container = GetElementInTable(hour, lesson.Day);
        if (container == null) {
            container = new Grid() {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = new SolidColorBrush(Colors.Transparent),
                Margin = new Thickness(5),
                MaxHeight = 120,
                Width = double.NaN
            };
            var flipView = new FlipView() {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = new SolidColorBrush(Colors.Transparent),
                Name = "FlipView",
                MaxWidth = 270,
                Width = double.NaN,
                MinWidth = 100
            };
            var pipsPager = new PipsPager() {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 5),
                NumberOfPages = 0,
                SelectedPageIndex = flipView.SelectedIndex
            };
            flipView.SelectionChanged += (sender, args) => {
                pipsPager.SelectedPageIndex = flipView.SelectedIndex;
            };
            pipsPager.SelectedIndexChanged += (sender, args) => {
                flipView.SelectedIndex = pipsPager.SelectedPageIndex;
            };
            container.Children.Add(flipView);
            container.Children.Add(pipsPager);

            Grid.SetRow(container, hour);
            Grid.SetColumn(container, lesson.Day);
            TimeTableGrid.Children.Add(container);
        }

        var flipViewChild = container.FindChild<FlipView>();
        var pipsPagerChild = container.FindChild<PipsPager>();
        if (flipViewChild == null || pipsPagerChild == null) return;
        flipViewChild.Items.Add(border);
        if (flipViewChild.Items.Count >= 2) {
            pipsPagerChild.NumberOfPages = flipViewChild.Items.Count;
        }
    }

    public Grid? GetElementInTable(int row, int column) {
        foreach (UIElement element in TimeTableGrid.Children) {
            if (element is not Grid) continue;
            Grid grid = (Grid)element;
            if (Grid.GetRow(grid) == row && Grid.GetColumn(grid) == column) {
                return grid;
            }
        }
        return null;
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
