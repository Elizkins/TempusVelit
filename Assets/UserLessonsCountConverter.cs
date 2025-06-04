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
    public class UserLessonsCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is User)
            {
                var userLessonsCount = MainPage.User.UserLessons.Count(ul => ul.IsCompleted == true);
                int allCount = TempusVelitData.Context.Lessons.Count();

                return $"ПРОЙДЕНО {userLessonsCount} ИЗ {allCount} УРОКОВ";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
