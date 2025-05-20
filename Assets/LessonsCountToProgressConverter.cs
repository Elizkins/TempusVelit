using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class LessonsCountToProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection<Lesson> lessons)
            {
                int moduleId = lessons.ToList()[0].ModuleID;
                var userLessonsCount = MainPage.User.UserLesson.Where(ul => ul.IsCompleted == true && ul.Lesson.ModuleID == moduleId).Count();
                int result = (int)((Double)userLessonsCount / (Double)lessons.Count * 100);
                return result;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
