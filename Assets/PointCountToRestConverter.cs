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
    public class PointCountToRestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is User user))
                return null;

            int pountCount = 100 - user.PointCount % 100;

            return $"ЕЩЕ {pountCount} {TempusVelitData.GetDenclensionString(pountCount, "БАЛЛ", "БАЛЛА", "БАЛЛОВ")}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
