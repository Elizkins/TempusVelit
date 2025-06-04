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

namespace TempusVelit.Assets
{
    public class AnswerHighlightToBackgroundConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || !(values[0] is bool) || !(values[1] is bool?))
                return (Brush)Application.Current.Resources["LightBaseColor"];

            bool isNeedToHightLight = (bool)values[0];
            bool? isCorrect = (bool?)values[1];

            Debug.WriteLine($"{isNeedToHightLight}     {isCorrect}");
            if (isNeedToHightLight)
            {
                return isCorrect == true
                    ? (Brush)Application.Current.Resources["TruthColor"]
                    : (Brush)Application.Current.Resources["ErrorColor"];
            }

            return (Brush)Application.Current.Resources["LightBaseColor"];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
