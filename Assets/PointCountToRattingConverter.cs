using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TempusVelit.Database;

namespace TempusVelit.Assets
{
    public class PointCountToRattingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is User user))
                return null;

            int index = TempusVelitData.Context.Users.OrderByDescending(u => u.PointCount).ToList().FindIndex(i => i == user);

            return ++index;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
