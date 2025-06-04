using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TempusVelit.Database;
using TempusVelit.Pages;

namespace TempusVelit.Assets
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Achievement achievement)
            {
                if (MainPage.User.UserAchievements.Any(ua => ua.Achievement.AchievementID == achievement.AchievementID))
                {
                    Debug.WriteLine("dsfsd");
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
