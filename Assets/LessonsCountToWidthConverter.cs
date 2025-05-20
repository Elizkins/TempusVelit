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
    public class LessonsCountToWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || !(values[0] is ICollection<Lesson> lessons) || !(values[1] is double width))
                return 0;

            if (!lessons.Any())
                return 0;

            int moduleId = lessons.First().ModuleID;
            var completedCount = MainPage.User.UserLesson
                .Count(ul => ul.IsCompleted == true && ul.Lesson.ModuleID == moduleId);

            double progress = (double)completedCount / lessons.Count;
            double resultWidth = progress * width;

            return Math.Max(0, Math.Min(resultWidth, width));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
