using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TempusVelit.Database;
using TempusVelit.Pages;

namespace TempusVelit.Assets
{
    public class EmailExistConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string email = value?.ToString();

            if (MainPage.User?.Email == email)
            {
                return false;
            }

            try
            {
                return TempusVelitData.Context.Users.Any(u => u.Email == email);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
