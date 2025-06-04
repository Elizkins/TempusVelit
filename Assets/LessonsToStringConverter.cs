using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TempusVelit.Database;
using TempusVelit.Pages;

namespace TempusVelit.Assets
{
    public class LessonsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection<Lesson> lessons)
            {
                int moduleId = lessons.ToList()[0].ModuleID;
                var userLessons = MainPage.User.UserLessons.Where(ul => ul.Lesson.ModuleID == moduleId).OrderBy(l => l.Lesson.OrderNumber).ToList();
                int lessonsCount = lessons.Count;
                if (userLessons.Count() == lessonsCount)
                {
                    return $"Завершен: {userLessons.ElementAt(userLessons.Count() - 1).CompletedDate?.ToString("dd.MM.yyyy")}";
                }
                else if (userLessons.Count() == 0)
                {
                    return $"Не начат";
                }
                else
                {
                    return $"Начат: {userLessons.ElementAt(0).CompletedDate?.ToString("dd.MM.yyyy")}";
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
