using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TempusVelit.Database;
using TempusVelit.Pages;

namespace TempusVelit.Assets
{
    public class UserLessonToEllipseColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection<UserLesson> userLessons)
            {
                int currentUserId = MainPage.User.UserID;

                bool isCurrentLesson = userLessons.Any(ul => ul.UserID == currentUserId & ul.IsCompleted == false);

                bool hasUserLesson = userLessons.Any(ul => ul.UserID == currentUserId & ul.IsCompleted == true);

                if (isCurrentLesson)
                {
                    var drawingBrush = new DrawingBrush
                    {
                        TileMode = TileMode.Tile,
                        Viewport = new Rect(0, 0, 5, 5),
                        ViewportUnits = BrushMappingMode.Absolute,
                        Drawing = new GeometryDrawing
                        {
                            Geometry = Geometry.Parse("M0,0 L5, 5"),
                            Pen = new Pen
                            {
                                Brush = (SolidColorBrush)Application.Current.Resources["DarkBaseColor"],
                                Thickness = 0.5
                            }
                        }
                    };
                    return drawingBrush;
                }
                else
                {
                    return hasUserLesson
                             ? (SolidColorBrush)Application.Current.Resources["DarkBaseColor"]
                             : (SolidColorBrush)Application.Current.Resources["LightBaseColor"];
                }

            }

            return (SolidColorBrush)Application.Current.Resources["LightBaseColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
