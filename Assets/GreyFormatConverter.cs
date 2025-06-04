using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace TempusVelit.Assets
{
    public class GreyFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string uri)
            {
                var bitmap = new BitmapImage(new Uri(uri));
                var grayBitmap = new FormatConvertedBitmap(bitmap, PixelFormats.Gray8, null, 0);
                return grayBitmap;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
