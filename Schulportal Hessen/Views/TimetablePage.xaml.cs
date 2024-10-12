using CommunityToolkit.WinUI.UI;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Schulportal_Hessen.Helpers;
using Schulportal_Hessen.Models;
using Schulportal_Hessen.ViewModels;

namespace Schulportal_Hessen.Views;

public sealed partial class TimetablePage : Page
{
    public TimetableViewModel ViewModel
    {
        get;
    }

    public SpWrapper _SpWrapper
    {
        get;
    }

    public TimetablePage()
    {
        ViewModel = App.GetService<TimetableViewModel>();
        _SpWrapper = App.GetService<SpWrapper>();
        Loaded += TimetablePage_Loaded;
        InitializeComponent();
    }


    private async void TimetablePage_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadContents();
    }

    public async Task LoadContents()
    {
        if (!await _SpWrapper.AutoLoginAsync())
        {
            return;
        }
        TimetableHeader.Text = await _SpWrapper.GetSchoolClassAsync();
        var timeTableLessons = await _SpWrapper.GetTimetableAsync();
        foreach (var lesson in timeTableLessons)
        {
            var border = new Border();
            border.Background = new SolidColorBrush(Colors.LightBlue);
            border.CornerRadius = new CornerRadius(5);
            border.Margin = new Thickness(5);
            var container = new StackPanel()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var lessonName = new TextBlock
            {
                Text = lesson.Subject,
                FontSize = 18,
                FontWeight = FontWeights.SemiBold,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            container.Children.Add(lessonName);

            var roomContainer = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var roomFontIcon = new FontIcon()
            {
                Glyph = "\uE77B",
                FontSize = 10,
                Margin = new Thickness(0,0,5,0)
            };
            roomContainer.Children.Add(roomFontIcon);
            var lessonRoom = new TextBlock
            {
                Text = lesson.Room,
                FontSize = 15,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            roomContainer.Children.Add(lessonRoom);

            var teacherContainer = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var teacherFontIcon = new FontIcon()
            {
                Glyph = "\uE816",
                FontSize = 10,
                Margin = new Thickness(0, 0, 5, 0)
            };
            teacherContainer.Children.Add(teacherFontIcon);
            var lessonTeacher = new TextBlock
            {
                Text = lesson.Teacher,
                FontSize = 15,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            teacherContainer.Children.Add(lessonTeacher);

            container.Children.Add(teacherContainer);
            container.Children.Add(roomContainer);
            border.Child = container;
            Grid.SetRow(border, lesson.Hour);
            Grid.SetColumn(border, lesson.Day);
            TimeTableGrid.Children.Add(border);
            // TODO: HANDLE SEVERAL TIMETABLELESSONS ON SAME GRID POS

        }
    }
}
